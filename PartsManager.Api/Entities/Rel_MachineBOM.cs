using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartsManager.Api.Entities;

[Table("Rel_MachineBOM")]
public class Rel_MachineBOM
{
    [Key]
    public int ID { get; set; }

    public int MachineID { get; set; }
    [ForeignKey("MachineID")]
    public Mdm_Machines? Machine { get; set; }

    public int MaterialID { get; set; }
    [ForeignKey("MaterialID")]
    public Mdm_Materials? Material { get; set; }

    [MaxLength(50)]
    public string? UsageTiming { get; set; } // e.g., "3 Months Fixed"

    public int UsageQty { get; set; } // 注�?：Schema ?�述說是 int，�??�是 decimal?��??�裡?�為 int??
    public int RotateQty { get; set; }

    [MaxLength(500)]
    public string? Remarks { get; set; }
}
