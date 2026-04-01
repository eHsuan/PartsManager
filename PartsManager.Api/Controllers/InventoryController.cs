using PartsManager.Api.Data;
using PartsManager.Shared.DTOs;
using PartsManager.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PartsManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly AppDbContext _context;

    public InventoryController(IStockService stockService, AppDbContext context)
    {
        _stockService = stockService;
        _context = context;
    }

    [HttpGet("scan/{barcode}")]
    public async Task<ActionResult<MaterialStockInfoDto>> GetScan(string barcode)
    {
        string normalizedBarcode = barcode.Trim().ToLower();
        var material = await _context.Mdm_Materials
            .FirstOrDefaultAsync(m => m.BarCode.ToLower() == normalizedBarcode);

        if (material == null)
        {
            return NotFound($"找不到條碼為 {barcode} 的物料");
        }

        var stocks = await _context.Inv_CurrentStock
            .Include(s => s.Warehouse)
            .Where(s => s.MaterialID == material.MaterialID)
            .Select(s => new StockDetailDto
            {
                WarehouseId = s.WarehouseID,
                WarehouseName = s.Warehouse != null ? s.Warehouse.WarehouseCode : string.Empty,
                Quantity = s.Quantity
            })
            .ToListAsync();

        var result = new MaterialStockInfoDto
        {
            MaterialID = material.MaterialID,
            Barcode = material.BarCode ?? string.Empty,
            PartNo = material.PartNo,
            Name = material.Name,
            Stocks = stocks
        };

        return Ok(result);
    }

    [HttpPost("inbound")]
    public async Task<IActionResult> Inbound([FromBody] InboundDto dto)
    {
        try
        {
            await _stockService.InboundAsync(dto);
            return Ok(new { message = "入庫成功" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("outbound")]
    public async Task<ActionResult<OutboundResultDto>> Outbound([FromBody] OutboundDto dto)
    {
        var result = await _stockService.OutboundAsync(dto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<LowStockDto>>> GetLowStock()
    {
        var stockSummary = await _context.Inv_CurrentStock
            .GroupBy(s => s.MaterialID)
            .Select(g => new { MaterialID = g.Key, TotalQuantity = (decimal)g.Sum(s => (double)s.Quantity) })
            .ToListAsync();

        var materials = await _context.Mdm_Materials.ToListAsync();

        var result = (from m in materials
                      join s in stockSummary on m.MaterialID equals s.MaterialID into stockGroup
                      from s in stockGroup.DefaultIfEmpty()
                      let totalQty = s != null ? s.TotalQuantity : 0
                      where totalQty <= m.SafeStockQty
                      select new LowStockDto
                      {
                          MaterialID = m.MaterialID,
                          PartNo = m.PartNo,
                          Name = m.Name,
                          Specification = m.Specification,
                          TotalQuantity = totalQty,
                          SafeStockQty = m.SafeStockQty
                      }).ToList();

        return Ok(result);
    }

    [HttpPost("audit")]
    public async Task<IActionResult> Audit([FromBody] AuditDto dto)
    {
        try
        {
            await _stockService.AuditAsync(dto);
            return Ok(new { message = "調整成功" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("compare")]
    public async Task<ActionResult<IEnumerable<InventoryComparisonResultDto>>> Compare([FromBody] InventoryCheckRequestDto request)
    {
        // 1. 取得該倉庫的所有庫存
        var systemStocks = await (from s in _context.Inv_CurrentStock
                                  join m in _context.Mdm_Materials on s.MaterialID equals m.MaterialID
                                  where s.WarehouseID == request.WarehouseId
                                  select new
                                  {
                                      m.PartNo,
                                      m.Name,
                                      m.Specification,
                                      s.Quantity
                                  }).ToListAsync();

        var results = new List<InventoryComparisonResultDto>();

        // 2. 處理實盤資料 (Scanned)
        // 注意：同一 PartNo 可能被分多次掃描 (雖然前端應處理重複，但後端累加較保險)
        var scannedGroups = request.Items
            .GroupBy(i => i.PartNo)
            .Select(g => new { PartNo = g.Key, TotalScanned = g.Sum(i => i.ScannedQty) })
            .ToList();

        // 3. 以「實盤」為主進行比對
        foreach (var scan in scannedGroups)
        {
            var sys = systemStocks.FirstOrDefault(s => s.PartNo == scan.PartNo);
            var material = await _context.Mdm_Materials.FirstOrDefaultAsync(m => m.PartNo == scan.PartNo);

            results.Add(new InventoryComparisonResultDto
            {
                PartNo = scan.PartNo,
                Name = sys?.Name ?? material?.Name ?? "Unknown",
                Specification = sys?.Specification ?? material?.Specification ?? "",
                SystemQty = sys?.Quantity ?? 0,
                ScannedQty = scan.TotalScanned,
                Status = (sys == null) ? "Excess" : (sys.Quantity == scan.TotalScanned ? "Match" : "Different")
            });
        }

        // 4. 以「系統」為主找出未掃描到的項目 (Missing)
        foreach (var sys in systemStocks)
        {
            if (!results.Any(r => r.PartNo == sys.PartNo))
            {
                results.Add(new InventoryComparisonResultDto
                {
                    PartNo = sys.PartNo,
                    Name = sys.Name,
                    Specification = sys.Specification,
                    SystemQty = sys.Quantity,
                    ScannedQty = 0,
                    Status = "Missing"
                });
            }
        }

        return Ok(results.OrderBy(r => r.Status).ThenBy(r => r.PartNo));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<SparePartSearchResultDto>>> Search(string? query = null)
    {
        string normalizedQuery = (query ?? "").Trim().ToLower();

        var results = await (from m in _context.Mdm_Materials
                             join s in _context.Inv_CurrentStock on m.MaterialID equals s.MaterialID into stockGroup
                             from s in stockGroup.DefaultIfEmpty()
                             join w in _context.Mdm_Warehouses on s.WarehouseID equals w.WarehouseID into warehouseGroup
                             from w in warehouseGroup.DefaultIfEmpty()
                             where string.IsNullOrEmpty(normalizedQuery) ||
                                   m.PartNo.ToLower().Contains(normalizedQuery) ||
                                   m.Name.ToLower().Contains(normalizedQuery)
                             select new SparePartSearchResultDto
                             {
                                 MaterialId = m.MaterialID,
                                 PartNo = m.PartNo,
                                 Name = m.Name,
                                 Specification = m.Specification ?? string.Empty,
                                 Supplier = m.Supplier ?? string.Empty,
                                 Manufacturer = m.Manufacturer ?? string.Empty,
                                 SafeStockQty = m.SafeStockQty,
                                 Price = m.Price,
                                 TotalAmount = m.Price * (s != null ? s.Quantity : 0),
                                 LeadTimeDays = m.LeadTimeDays,
                                 Quantity = s != null ? s.Quantity : 0,
                                 WarehouseName = w != null ? $"{w.WarehouseCode} - {w.WarehouseName}" : "--- (無庫存紀錄)",
                                 AttachmentFileNames = _context.Mdm_MaterialAttachments
                                    .Where(a => a.MaterialID == m.MaterialID)
                                    .Select(a => a.FileName)
                                    .ToList()
                             }).ToListAsync();

        return Ok(results);
    }
}
