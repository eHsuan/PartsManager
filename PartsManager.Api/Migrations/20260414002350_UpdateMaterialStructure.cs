using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartsManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaterialStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Mdm_Materials");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "Mdm_Materials");

            migrationBuilder.AddColumn<string>(
                name: "StorageLocation",
                table: "Mdm_Materials",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 1,
                column: "StorageLocation",
                value: "");

            migrationBuilder.UpdateData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 2,
                column: "StorageLocation",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageLocation",
                table: "Mdm_Materials");

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
        }
    }
}
