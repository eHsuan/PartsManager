using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartsManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mdm_MaterialAttachments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaterialID = table.Column<int>(type: "INTEGER", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    UploadTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mdm_MaterialAttachments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Mdm_MaterialAttachments_Mdm_Materials_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Mdm_Materials",
                        principalColumn: "MaterialID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mdm_MaterialAttachments_MaterialID",
                table: "Mdm_MaterialAttachments",
                column: "MaterialID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mdm_MaterialAttachments");
        }
    }
}
