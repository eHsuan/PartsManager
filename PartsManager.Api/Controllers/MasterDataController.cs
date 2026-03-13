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
    private readonly string _storagePath;

    public MasterDataController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _storagePath = configuration["AttachmentStoragePath"] ?? "Attachments";
    }

    [HttpPost("materials/{id}/attachments")]
    public async Task<IActionResult> UploadAttachments(int id, IFormFileCollection files)
    {
        var material = await _context.Mdm_Materials.FindAsync(id);
        if (material == null) return NotFound("Material not found");

        var currentCount = await _context.Mdm_MaterialAttachments.CountAsync(a => a.MaterialID == id);
        if (currentCount + files.Count > 2)
        {
            return BadRequest("Maximum 2 attachments allowed per material.");
        }

        string materialFolder = Path.Combine(_storagePath, id.ToString());
        if (!Directory.Exists(materialFolder)) Directory.CreateDirectory(materialFolder);

        foreach (var file in files)
        {
            if (Path.GetExtension(file.FileName).ToLower() != ".pdf") continue;

            string filePath = Path.Combine(materialFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new Mdm_MaterialAttachments
            {
                MaterialID = id,
                FileName = file.FileName,
                FilePath = filePath,
                UploadTime = DateTime.Now
            };
            _context.Mdm_MaterialAttachments.Add(attachment);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("materials/{id}/attachments")]
    public async Task<ActionResult<IEnumerable<AttachmentDto>>> GetAttachments(int id)
    {
        return await _context.Mdm_MaterialAttachments
            .Where(a => a.MaterialID == id)
            .Select(a => new AttachmentDto
            {
                ID = a.ID,
                MaterialID = a.MaterialID,
                FileName = a.FileName,
                UploadTime = a.UploadTime
            }).ToListAsync();
    }

    [HttpGet("materials/{id}/attachments/{fileName}/download")]
    public async Task<IActionResult> DownloadAttachment(int id, string fileName)
    {
        var attachment = await _context.Mdm_MaterialAttachments
            .FirstOrDefaultAsync(a => a.MaterialID == id && a.FileName == fileName);
        
        if (attachment == null || !System.IO.File.Exists(attachment.FilePath))
            return NotFound();

        var bytes = await System.IO.File.ReadAllBytesAsync(attachment.FilePath);
        return File(bytes, "application/pdf", attachment.FileName);
    }

    [HttpDelete("materials/{id}/attachments/{fileName}")]
    public async Task<IActionResult> DeleteAttachment(int id, string fileName)
    {
        var attachment = await _context.Mdm_MaterialAttachments
            .FirstOrDefaultAsync(a => a.MaterialID == id && a.FileName == fileName);

        if (attachment != null)
        {
            if (System.IO.File.Exists(attachment.FilePath))
            {
                System.IO.File.Delete(attachment.FilePath);
            }
            _context.Mdm_MaterialAttachments.Remove(attachment);
            await _context.SaveChangesAsync();
        }
        return NoContent();
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
            Supplier = dto.Supplier,
            Manufacturer = dto.Manufacturer,
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
        material.Supplier = dto.Supplier;
        material.Manufacturer = dto.Manufacturer;
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
            Supplier = material.Supplier ?? "",
            Manufacturer = material.Manufacturer ?? "",
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
