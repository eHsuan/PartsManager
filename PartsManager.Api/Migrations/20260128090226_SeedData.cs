using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PartsManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Mdm_Machines",
                columns: new[] { "MachineID", "MachineCode", "MachineName" },
                values: new object[] { 1, "M01", "SMT-01" });

            migrationBuilder.InsertData(
                table: "Mdm_Materials",
                columns: new[] { "MaterialID", "BarCode", "Name", "PartNo", "SafeStockQty", "SourceType" },
                values: new object[,]
                {
                    { 1, "1001", "M3 Screw", "SCREW-001", 100, (byte)0 },
                    { 2, "1002", "Resistor 10k", "RES-10K", 500, (byte)0 }
                });

            migrationBuilder.InsertData(
                table: "Mdm_Warehouses",
                columns: new[] { "WarehouseID", "IsExternalMES", "WarehouseCode" },
                values: new object[,]
                {
                    { 1, false, "MainWH" },
                    { 2, false, "LineSideA" }
                });

            migrationBuilder.InsertData(
                table: "Inv_CurrentStock",
                columns: new[] { "StockID", "BinLocation", "LastUpdated", "MaterialID", "Quantity", "WarehouseID" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2026, 1, 28, 17, 2, 24, 882, DateTimeKind.Local).AddTicks(6835), 1, 500m, 1 },
                    { 2L, null, new DateTime(2026, 1, 28, 17, 2, 24, 882, DateTimeKind.Local).AddTicks(6853), 2, 1000m, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Inv_CurrentStock",
                keyColumn: "StockID",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Inv_CurrentStock",
                keyColumn: "StockID",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Mdm_Machines",
                keyColumn: "MachineID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Mdm_Warehouses",
                keyColumn: "WarehouseID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Mdm_Warehouses",
                keyColumn: "WarehouseID",
                keyValue: 1);
        }
    }
}

