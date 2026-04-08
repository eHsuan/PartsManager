using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartsManager.Api.Data;
using PartsManager.Api.Entities;
using PartsManager.Shared.DTOs;

namespace PartsManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MachineController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MachineDto>>> GetMachines()
        {
            return await _context.Mdm_Machines
                .Select(m => new MachineDto
                {
                    MachineID = m.MachineID,
                    MachineCode = m.MachineCode,
                    MachineName = m.MachineName ?? string.Empty
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MachineDto>> GetMachine(int id)
        {
            var machine = await _context.Mdm_Machines.FindAsync(id);
            if (machine == null) return NotFound();

            return new MachineDto
            {
                MachineID = machine.MachineID,
                MachineCode = machine.MachineCode,
                MachineName = machine.MachineName ?? string.Empty
            };
        }

        [HttpPost]
        public async Task<ActionResult<MachineDto>> CreateMachine(CreateMachineDto dto)
        {
            var machine = new Mdm_Machines
            {
                MachineCode = dto.MachineCode,
                MachineName = dto.MachineName
            };

            _context.Mdm_Machines.Add(machine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMachine), new { id = machine.MachineID }, new MachineDto
            {
                MachineID = machine.MachineID,
                MachineCode = machine.MachineCode,
                MachineName = machine.MachineName ?? string.Empty
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(int id, UpdateMachineDto dto)
        {
            var machine = await _context.Mdm_Machines.FindAsync(id);
            if (machine == null) return NotFound();

            machine.MachineCode = dto.MachineCode;
            machine.MachineName = dto.MachineName;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            var machine = await _context.Mdm_Machines.FindAsync(id);
            if (machine == null) return NotFound();

            // 檢查是否有 BOM 關聯，若有則不允許刪除（或連動刪除，此處採保守策略）
            bool hasBOM = await _context.Rel_MachineBOM.AnyAsync(r => r.MachineID == id);
            if (hasBOM)
            {
                return BadRequest("該機台已有關聯物料，無法刪除。");
            }

            _context.Mdm_Machines.Remove(machine);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
