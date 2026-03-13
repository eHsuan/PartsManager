using PartsManager.Api.Data;
using PartsManager.Api.Entities;
using PartsManager.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PartsManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto dto)
    {
        // 強制重置機制：確保系統永遠有一個有效的 admin/admin (Level 1)
        var existingAdmin = await _context.Sys_Users.FirstOrDefaultAsync(u => u.Username == "admin");
        if (existingAdmin != null)
        {
            _context.Sys_Users.Remove(existingAdmin);
            await _context.SaveChangesAsync();
        }

        var admin = new Sys_Users
        {
            Username = "admin",
            PasswordHash = "admin",
            UserLevel = 1,
            IsActive = true
        };
        _context.Sys_Users.Add(admin);
        await _context.SaveChangesAsync();

        // 執行實際登入比對
        var user = await _context.Sys_Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username && u.PasswordHash == dto.Password);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        return new UserDto
        {
            UserID = user.UserID,
            Username = user.Username,
            UserLevel = user.UserLevel,
            IsActive = user.IsActive
        };
    }
}
