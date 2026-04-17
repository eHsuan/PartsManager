using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartsManager.Api.Migrations
{
    public partial class AddTransactionSnapshots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartNo",
                table: "Inv_Transactions",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialName",
                table: "Inv_Transactions",
                type: "TEXT",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartNo",
                table: "Inv_Transactions");

            migrationBuilder.DropColumn(
                name: "MaterialName",
                table: "Inv_Transactions");
        }
    }
}
