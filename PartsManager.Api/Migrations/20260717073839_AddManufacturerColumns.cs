using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartsManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddManufacturerColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Mdm_Materials",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManufacturerPartNo",
                table: "Mdm_Materials",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialName",
                table: "Inv_Transactions",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartNo",
                table: "Inv_Transactions",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 1,
                columns: new[] { "Manufacturer", "ManufacturerPartNo" },
                values: new object[] { "Generic MFG", "MFG-SCREW-M3" });

            migrationBuilder.UpdateData(
                table: "Mdm_Materials",
                keyColumn: "MaterialID",
                keyValue: 2,
                columns: new[] { "Manufacturer", "ManufacturerPartNo" },
                values: new object[] { "Yageo", "RC0603JR-0710KL" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Mdm_Materials");

            migrationBuilder.DropColumn(
                name: "ManufacturerPartNo",
                table: "Mdm_Materials");

            migrationBuilder.DropColumn(
                name: "MaterialName",
                table: "Inv_Transactions");

            migrationBuilder.DropColumn(
                name: "PartNo",
                table: "Inv_Transactions");
        }
    }
}
