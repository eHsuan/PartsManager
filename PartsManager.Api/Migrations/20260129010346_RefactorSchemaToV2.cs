using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartsManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSchemaToV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BinLocation",
                table: "Inv_CurrentStock");

            migrationBuilder.AlterColumn<int>(
                name: "UsageQty",
                table: "Rel_MachineBOM",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Rel_MachineBOM",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RotateQty",
                table: "Rel_MachineBOM",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UsageTiming",
                table: "Rel_MachineBOM",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseName",
                table: "Mdm_Warehouses",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncTime",
                table: "Mdm_Materials",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeadTimeDays",
                table: "Mdm_Materials",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsPrintLabel",
                table: "Mdm_Materials",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Specification",
                table: "Mdm_Materials",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatorID",
                table: "Inv_Transactions",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseID",
                table: "Inv_Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Sys_SyncLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaskName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    RecordsProcessed = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_SyncLogs", x => x.LogID);
                });

            migrationBuilder.UpdateData(
                table: "Inv_CurrentStock",
                keyColumn: "StockID",
                keyValue: 1L,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 28, 17, 2, 24, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Inv_CurrentStock",
                keyColumn: "StockID",
                keyValue: 2L,
                column: "LastUpdated",
                value: new DateTime(2026, 1, 28, 17, 2, 24, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 1,
                columns: new[] { "LastSyncTime", "LeadTimeDays", "NeedsPrintLabel", "Specification" },
                values: new object[] { null, 3, true, "Stainless Steel M3x10" });

            migrationBuilder.UpdateData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 2,
                columns: new[] { "LastSyncTime", "LeadTimeDays", "NeedsPrintLabel", "Specification" },
                values: new object[] { null, 7, true, "10k Ohm 0603" });

            migrationBuilder.UpdateData(
                table: "Mdm_Warehouses",
                keyColumn: "WarehouseID",
                keyValue: 1,
                column: "WarehouseName",
                value: "�D�ܮw (Main)");

            migrationBuilder.UpdateData(
                table: "Mdm_Warehouses",
                keyColumn: "WarehouseID",
                keyValue: 2,
                column: "WarehouseName",
                value: "�u��� A");

            migrationBuilder.CreateIndex(
                name: "IX_Inv_Transactions_WarehouseID",
                table: "Inv_Transactions",
                column: "WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inv_Transactions_Mdm_Warehouses_WarehouseID",
                table: "Inv_Transactions",
                column: "WarehouseID",
                principalTable: "Mdm_Warehouses",
                principalColumn: "WarehouseID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inv_Transactions_Mdm_Warehouses_WarehouseID",
                table: "Inv_Transactions");

            migrationBuilder.DropTable(
                name: "Sys_SyncLogs");

            migrationBuilder.DropIndex(
                name: "IX_Inv_Transactions_WarehouseID",
                table: "Inv_Transactions");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Rel_MachineBOM");

            migrationBuilder.DropColumn(
                name: "RotateQty",
                table: "Rel_MachineBOM");

            migrationBuilder.DropColumn(
                name: "UsageTiming",
                table: "Rel_MachineBOM");

            migrationBuilder.DropColumn(
                name: "WarehouseName",
                table: "Mdm_Warehouses");

            migrationBuilder.DropColumn(
                name: "LastSyncTime",
                table: "Mdm_Materials");

            migrationBuilder.DropColumn(
                name: "LeadTimeDays",
                table: "Mdm_Materials");

            migrationBuilder.DropColumn(
                name: "NeedsPrintLabel",
                table: "Mdm_Materials");

            migrationBuilder.DropColumn(
                name: "Specification",
                table: "Mdm_Materials");

            migrationBuilder.DropColumn(
                name: "OperatorID",
                table: "Inv_Transactions");

            migrationBuilder.DropColumn(
                name: "WarehouseID",
                table: "Inv_Transactions");

            migrationBuilder.AlterColumn<decimal>(
                name: "UsageQty",
                table: "Rel_MachineBOM",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "BinLocation",
                table: "Inv_CurrentStock",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Inv_CurrentStock",
                keyColumn: "StockID",
                keyValue: 1L,
                columns: new[] { "BinLocation", "LastUpdated" },
                values: new object[] { null, new DateTime(2026, 1, 28, 17, 2, 24, 882, DateTimeKind.Local).AddTicks(6835) });

            migrationBuilder.UpdateData(
                table: "Inv_CurrentStock",
                keyColumn: "StockID",
                keyValue: 2L,
                columns: new[] { "BinLocation", "LastUpdated" },
                values: new object[] { null, new DateTime(2026, 1, 28, 17, 2, 24, 882, DateTimeKind.Local).AddTicks(6853) });
        }
    }
}

