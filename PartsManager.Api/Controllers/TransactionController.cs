using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartsManager.Api.Data;
using PartsManager.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartsManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("history")]
    public async Task<ActionResult<List<TransactionDto>>> GetHistory([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        // 確保結束日期包含整天
        var endDate = end.Date.AddDays(1).AddTicks(-1);
        var startDate = start.Date;

        var queries = await _context.Inv_Transactions
            .Include(t => t.Material)
            .Include(t => t.Warehouse)
            .Where(t => t.TransTime >= startDate && t.TransTime <= endDate)
            .OrderByDescending(t => t.TransTime)
            .Select(t => new TransactionDto
            {
                TransID = t.TransID,
                TransType = t.TransType,
                // 如果快照沒值(舊資料)，就從關聯表抓
                PartNo = t.PartNo ?? (t.Material != null ? t.Material.PartNo : "N/A"),
                MaterialName = t.MaterialName ?? (t.Material != null ? t.Material.Name : "N/A"),
                ChangeQty = t.ChangeQty,
                AfterQty = t.AfterQty,
                ReasonCode = t.ReasonCode,
                OperatorID = t.OperatorID,
                TransTime = t.TransTime,
                WarehouseName = t.Warehouse != null ? t.Warehouse.WarehouseName : "N/A"
            })
            .ToListAsync();

        return queries;
    }
}
