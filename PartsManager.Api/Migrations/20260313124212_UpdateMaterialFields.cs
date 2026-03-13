using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartsManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaterialFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Station",
                table: "Mdm_Materials");

            migrationBuilder.AlterColumn<string>(
                name: "Specification",
                table: "Mdm_Materials",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Mdm_Materials",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "Mdm_Materials",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Sys_Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    UserLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Users", x => x.UserID);
                });

            migrationBuilder.UpdateData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 1,
                columns: new[] { "Manufacturer", "Supplier" },
                values: new object[] { "None", "None" });

            migrationBuilder.UpdateData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 2,
                columns: new[] { "Manufacturer", "Supplier" },
                values: new object[] { "None", "None" });

            migrationBuilder.UpdateData(
                table: "Mdm_Warehouses",
                keyColumn: "WarehouseID",
                keyValue: 1,
                column: "WarehouseName",
                value: "主倉庫 (Main)");

            migrationBuilder.UpdateData(
                table: "Mdm_Warehouses",
                keyColumn: "WarehouseID",
                keyValue: 2,
                column: "WarehouseName",
                value: "線邊倉 A");

            migrationBuilder.InsertData(
                table: "Sys_Users",
                columns: new[] { "UserID", "IsActive", "PasswordHash", "UserLevel", "Username" },
                values: new object[] { 1, true, "admin", 1, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sys_Users");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Mdm_Materials");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "Mdm_Materials");

            migrationBuilder.AlterColumn<string>(
                name: "Specification",
                table: "Mdm_Materials",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "Station",
                table: "Mdm_Materials",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 1,
                column: "Station",
                value: null);

            migrationBuilder.UpdateData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 2,
                column: "Station",
                value: null);

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
        }
    }
}
