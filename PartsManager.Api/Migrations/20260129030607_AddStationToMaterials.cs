using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartsManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStationToMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Station",
                table: "Mdm_Materials");
        }
    }
}

