using PartsManager.Api.Data;
using PartsManager.Api.Entities;
using PartsManager.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PartsManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MasterDataController : ControllerBase
{
    private readonly AppDbContext _context;

    public MasterDataController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("materials")]
    public async Task<ActionResult<MaterialDto>> CreateMaterial(CreateMaterialDto dto)
    {
        if (await _context.Mdm_Materials.AnyAsync(m => m.PartNo == dto.PartNo))
        {
            return Conflict(new { message = $"料號 {dto.PartNo} 已存在" });
        }

        var material = new Mdm_Materials
        {
            PartNo = dto.PartNo,
            Name = dto.Name,
            Specification = dto.Specification,
            Station = dto.Station,
            SafeStockQty = dto.SafeStockQty,
            LeadTimeDays = dto.LeadTimeDays,
            SourceType = dto.SourceType,
            BarCode = dto.PartNo.ToLower(), // 強制轉小寫存入，避免刷碼大小寫問題
            NeedsPrintLabel = true
        };

        _context.Mdm_Materials.Add(material);
        await _context.SaveChangesAsync();

        var result = new MaterialDto
        {
            MaterialID = material.MaterialID,
            BarCode = material.BarCode,
            PartNo = material.PartNo,
            Name = material.Name,
            Specification = material.Specification,
            SafeStockQty = material.SafeStockQty
        };

        return StatusCode(201, result);
    }

    [HttpPut("materials/{id}")]
    public async Task<IActionResult> UpdateMaterial(int id, UpdateMaterialDto dto)
    {
        var material = await _context.Mdm_Materials.FindAsync(id);
        if (material == null) return NotFound();

        if (material.PartNo != dto.PartNo)
        {
            if (await _context.Mdm_Materials.AnyAsync(m => m.PartNo == dto.PartNo))
            {
                return Conflict(new { message = $"料號 {dto.PartNo} 已存在" });
            }
        }

        material.PartNo = dto.PartNo;
        material.Name = dto.Name;
        material.Specification = dto.Specification;
        material.Station = dto.Station;
        material.SafeStockQty = dto.SafeStockQty;
        material.LeadTimeDays = dto.LeadTimeDays;
        material.BarCode = dto.PartNo.ToLower();

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("materials/{id}")]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        var material = await _context.Mdm_Materials.FindAsync(id);
        if (material == null) return NotFound();

        _context.Mdm_Materials.Remove(material);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("materials/{id}")]
    public async Task<ActionResult<MaterialDto>> GetMaterial(int id)
    {
        var material = await _context.Mdm_Materials.FindAsync(id);
        if (material == null) return NotFound();

        return new MaterialDto
        {
            MaterialID = material.MaterialID,
            BarCode = material.BarCode ?? "",
            PartNo = material.PartNo,
            Name = material.Name,
            Specification = material.Specification ?? "",
            Station = material.Station ?? "",
            SafeStockQty = material.SafeStockQty,
            LeadTimeDays = material.LeadTimeDays
        };
    }

    [HttpGet("machines")]
    public async Task<ActionResult<IEnumerable<Mdm_Machines>>> GetMachines()
    {
        return await _context.Mdm_Machines.ToListAsync();
    }

    [HttpGet("warehouses")]
    public async Task<ActionResult<IEnumerable<Mdm_Warehouses>>> GetWarehouses()
    {
        return await _context.Mdm_Warehouses.ToListAsync();
    }
}
