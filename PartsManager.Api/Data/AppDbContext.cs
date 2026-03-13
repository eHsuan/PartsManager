using Microsoft.EntityFrameworkCore;
using PartsManager.Api.Entities;

namespace PartsManager.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Mdm_Materials> Mdm_Materials { get; set; }
    public DbSet<Mdm_Warehouses> Mdm_Warehouses { get; set; }
    public DbSet<Mdm_Machines> Mdm_Machines { get; set; }
    public DbSet<Inv_CurrentStock> Inv_CurrentStock { get; set; }
    public DbSet<Inv_Transactions> Inv_Transactions { get; set; }
    public DbSet<Rel_MachineBOM> Rel_MachineBOM { get; set; }
    public DbSet<Sys_SyncLogs> Sys_SyncLogs { get; set; }
    public DbSet<Sys_Users> Sys_Users { get; set; }
    public DbSet<Mdm_MaterialAttachments> Mdm_MaterialAttachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Users
        modelBuilder.Entity<Sys_Users>().HasData(
            new Sys_Users { UserID = 1, Username = "admin", PasswordHash = "admin", UserLevel = 1, IsActive = true }
        );

        // Seed Warehouses
        modelBuilder.Entity<Mdm_Warehouses>().HasData(
            new Mdm_Warehouses { WarehouseID = 1, WarehouseCode = "MainWH", WarehouseName = "主倉庫 (Main)", IsExternalMES = false },
            new Mdm_Warehouses { WarehouseID = 2, WarehouseCode = "LineSideA", WarehouseName = "線邊倉 A", IsExternalMES = false }
        );

        // Seed Machines
        modelBuilder.Entity<Mdm_Machines>().HasData(
            new Mdm_Machines { MachineID = 1, MachineCode = "M01", MachineName = "SMT-01" }
        );

        // Seed Materials
        modelBuilder.Entity<Mdm_Materials>().HasData(
            new Mdm_Materials 
            { 
                MaterialID = 1, 
                PartNo = "SCREW-001", 
                Name = "M3 Screw", 
                BarCode = "1001", 
                SafeStockQty = 100, 
                SourceType = 0,
                Specification = "Stainless Steel M3x10",
                NeedsPrintLabel = true,
                LeadTimeDays = 3
            },
            new Mdm_Materials 
            { 
                MaterialID = 2, 
                PartNo = "RES-10K", 
                Name = "Resistor 10k", 
                BarCode = "1002", 
                SafeStockQty = 500, 
                SourceType = 0,
                Specification = "10k Ohm 0603",
                NeedsPrintLabel = true,
                LeadTimeDays = 7
            }
        );

        // Seed Initial Stock
        modelBuilder.Entity<Inv_CurrentStock>().HasData(
            new Inv_CurrentStock { StockID = 1L, MaterialID = 1, WarehouseID = 1, Quantity = 500, LastUpdated = new DateTime(2026, 1, 28, 17, 2, 24) },
            new Inv_CurrentStock { StockID = 2L, MaterialID = 2, WarehouseID = 1, Quantity = 1000, LastUpdated = new DateTime(2026, 1, 28, 17, 2, 24) }
        );
    }
}
