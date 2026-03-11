using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsManager.Api.Entities;

[Table("Sys_Users")]
public class Sys_Users
{
    [Key]
    public int UserID { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("PasswordHash")] // 強制對應到您 SQL 中的 PasswordHash
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public int UserLevel { get; set; } // 1: Admin, 2: Manager, 3: Operator, 4: Worker

    public bool IsActive { get; set; } = true;
}
