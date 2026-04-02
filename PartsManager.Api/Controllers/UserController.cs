using PartsManager.Api.Data;
using PartsManager.Api.Entities;
using PartsManager.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PartsManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        return await _context.Sys_Users
            .Select(u => new UserDto
            {
                UserID = u.UserID,
                Username = u.Username,
                UserLevel = u.UserLevel,
                IsActive = u.IsActive
            })
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromHeader(Name = "X-User-Level")] int requesterLevel, CreateUserDto dto)
    {
        if (requesterLevel != 1) return Forbid();

        if (await _context.Sys_Users.AnyAsync(u => u.Username == dto.Username))
        {
            return Conflict(new { message = "帳號已存在" });
        }

        var user = new Sys_Users
        {
            Username = dto.Username,
            PasswordHash = dto.Password,
            UserLevel = dto.UserLevel,
            IsActive = true
        };

        _context.Sys_Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            UserID = user.UserID,
            Username = user.Username,
            UserLevel = user.UserLevel,
            IsActive = user.IsActive
        };
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromHeader(Name = "X-User-Level")] int requesterLevel, int id)
    {
        if (requesterLevel != 1) return Forbid();

        var user = await _context.Sys_Users.FindAsync(id);
        if (user == null) return NotFound();
        if (user.Username == "admin") return BadRequest(new { message = "不能刪除主管理員" });

        _context.Sys_Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var user = await _context.Sys_Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user == null) return NotFound(new { message = "找不到使用者" });

        // 比對舊密碼
        if (user.PasswordHash != dto.OldPassword)
        {
            return BadRequest(new { message = "舊密碼不正確" });
        }

        // 更新新密碼
        user.PasswordHash = dto.NewPassword;
        await _context.SaveChangesAsync();

        return Ok(new { message = "密碼變更成功" });
    }
}
