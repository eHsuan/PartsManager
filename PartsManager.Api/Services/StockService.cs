using PartsManager.Api.Data;
using PartsManager.Shared.DTOs;
using PartsManager.Api.Entities;
using PartsManager.Shared.Resources;
using Microsoft.EntityFrameworkCore;

namespace PartsManager.Api.Services;

public class StockService : IStockService
{
    private readonly AppDbContext _context;

    public StockService(AppDbContext context)
    {
        _context = context;
    }

    public async Task InboundAsync(InboundDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            string normalizedBarcode = dto.Barcode.Trim().ToLower();
            var material = await _context.Mdm_Materials
                .FirstOrDefaultAsync(m => m.BarCode.ToLower() == normalizedBarcode);

            if (material == null)
            {
                string msg = string.Format(PartsManager.Shared.Resources.LocalizationService.GetString("Error_MaterialNotFound"), dto.Barcode);
                throw new InvalidOperationException(msg);
            }

            var stock = await _context.Inv_CurrentStock
                .FirstOrDefaultAsync(s => s.MaterialID == material.MaterialID && s.WarehouseID == dto.WarehouseId);

            decimal afterQty = 0;

            if (stock != null)
            {
                stock.Quantity += dto.Quantity;
                stock.LastUpdated = DateTime.Now;
                afterQty = stock.Quantity;
                _context.Inv_CurrentStock.Update(stock);
            }
            else
            {
                stock = new Inv_CurrentStock
                {
                    MaterialID = material.MaterialID,
                    WarehouseID = dto.WarehouseId,
                    Quantity = dto.Quantity,
                    LastUpdated = DateTime.Now
                };
                afterQty = dto.Quantity;
                await _context.Inv_CurrentStock.AddAsync(stock);
            }

            var trans = new Inv_Transactions
            {
                TransType = "IN",
                MaterialID = material.MaterialID,
                WarehouseID = dto.WarehouseId,
                ChangeQty = dto.Quantity,
                AfterQty = afterQty,
                ReasonCode = "Inbound", 
                OperatorID = dto.OperatorId,
                TransTime = DateTime.Now
            };
            await _context.Inv_Transactions.AddAsync(trans);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<OutboundResultDto> OutboundAsync(OutboundDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            string normalizedBarcode = dto.Barcode.Trim().ToLower();
            var material = await _context.Mdm_Materials
                .FirstOrDefaultAsync(m => m.BarCode.ToLower() == normalizedBarcode);

            if (material == null)
            {
                string msg = string.Format(PartsManager.Shared.Resources.LocalizationService.GetString("Error_MaterialNotFound"), dto.Barcode);
                throw new InvalidOperationException(msg);
            }

            var stock = await _context.Inv_CurrentStock
                .FirstOrDefaultAsync(s => s.MaterialID == material.MaterialID && s.WarehouseID == dto.WarehouseId);

            if (stock == null || stock.Quantity < dto.Quantity)
            {
                throw new InvalidOperationException($"庫存不足！目前庫存 {(stock?.Quantity ?? 0)}，請求 {dto.Quantity}");
            }

            stock.Quantity -= dto.Quantity;
            stock.LastUpdated = DateTime.Now;
            var afterQty = stock.Quantity;
            _context.Inv_CurrentStock.Update(stock);

            var trans = new Inv_Transactions
            {
                TransType = "OUT",
                MaterialID = material.MaterialID,
                WarehouseID = dto.WarehouseId,
                ChangeQty = -dto.Quantity,
                AfterQty = afterQty,
                ReasonCode = dto.ReasonCode ?? "Outbound",
                OperatorID = dto.OperatorId,
                TransTime = DateTime.Now
            };
            await _context.Inv_Transactions.AddAsync(trans);

            await _context.SaveChangesAsync();

            // 檢查全廠總庫存 (SQLite 限制：需轉為 double 計算 Sum)
            var totalQtyDouble = await _context.Inv_CurrentStock
                .Where(s => s.MaterialID == material.MaterialID)
                .SumAsync(s => (double)s.Quantity);

            decimal totalQty = (decimal)totalQtyDouble;

            bool isLowStock = totalQty <= material.SafeStockQty;
            string lowStockMessage = string.Empty;

            if (isLowStock)
            {
                string template = PartsManager.Shared.Resources.LocalizationService.GetString("Msg_LowStockAlert");
                if (string.IsNullOrEmpty(template))
                {
                    template = "全廠總庫存僅剩 {0}，已達安全水位 ({1})！請通知採購。";
                }
                lowStockMessage = string.Format(template, totalQty, material.SafeStockQty);
            }

            await transaction.CommitAsync();

            return new OutboundResultDto
            {
                IsSuccess = true,
                Message = "Outbound successful",
                IsLowStock = isLowStock,
                LowStockMessage = lowStockMessage
            };
        }
        catch (Exception ex)
        {
            // 由於邏輯已移至 Commit 前，此處發生異常時交易必屬未完成狀態，可安全回滾
            await transaction.RollbackAsync();
            
            return new OutboundResultDto
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    public async Task AuditAsync(AuditDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            string normalizedBarcode = dto.Barcode.Trim().ToLower();
            var material = await _context.Mdm_Materials
                .FirstOrDefaultAsync(m => m.BarCode.ToLower() == normalizedBarcode);

            if (material == null)
            {
                string msg = string.Format(PartsManager.Shared.Resources.LocalizationService.GetString("Error_MaterialNotFound"), dto.Barcode);
                throw new InvalidOperationException(msg);
            }

            var stock = await _context.Inv_CurrentStock
                .FirstOrDefaultAsync(s => s.MaterialID == material.MaterialID && s.WarehouseID == dto.WarehouseId);

            decimal beforeQty = stock?.Quantity ?? 0;
            decimal diff = dto.CountQty - beforeQty;

            if (stock != null)
            {
                stock.Quantity = dto.CountQty;
                stock.LastUpdated = DateTime.Now;
                _context.Inv_CurrentStock.Update(stock);
            }
            else
            {
                stock = new Inv_CurrentStock
                {
                    MaterialID = material.MaterialID,
                    WarehouseID = dto.WarehouseId,
                    Quantity = dto.CountQty,
                    LastUpdated = DateTime.Now
                };
                await _context.Inv_CurrentStock.AddAsync(stock);
            }

            var trans = new Inv_Transactions
            {
                TransType = "ADJ",
                MaterialID = material.MaterialID,
                WarehouseID = dto.WarehouseId,
                ChangeQty = diff,
                AfterQty = dto.CountQty,
                ReasonCode = dto.ReasonCode ?? "Audit",
                OperatorID = dto.OperatorId,
                TransTime = DateTime.Now
            };
            await _context.Inv_Transactions.AddAsync(trans);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
