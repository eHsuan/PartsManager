using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsManager.Api.Entities;

[Table("Mdm_Machines")]
public class Mdm_Machines
{
    [Key]
    public int MachineID { get; set; }

    [Required]
    [MaxLength(50)]
    public string MachineCode { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? MachineName { get; set; }
}
