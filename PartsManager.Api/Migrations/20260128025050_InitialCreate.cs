using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartsManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mdm_Machines",
                columns: table => new
                {
                    MachineID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MachineCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MachineName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mdm_Machines", x => x.MachineID);
                });

            migrationBuilder.CreateTable(
                name: "Mdm_Materials",
                columns: table => new
                {
                    MaterialID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BarCode = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PartNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SourceType = table.Column<byte>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SafeStockQty = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mdm_Materials", x => x.MaterialID);
                });

            migrationBuilder.CreateTable(
                name: "Mdm_Warehouses",
                columns: table => new
                {
                    WarehouseID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WarehouseCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsExternalMES = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mdm_Warehouses", x => x.WarehouseID);
                });

            migrationBuilder.CreateTable(
                name: "Inv_Transactions",
                columns: table => new
                {
                    TransID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransType = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    MaterialID = table.Column<int>(type: "INTEGER", nullable: false),
                    ChangeQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AfterQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReasonCode = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    TransTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inv_Transactions", x => x.TransID);
                    table.ForeignKey(
                        name: "FK_Inv_Transactions_Mdm_Materials_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Mdm_Materials",
                        principalColumn: "MaterialID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rel_MachineBOM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MachineID = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialID = table.Column<int>(type: "INTEGER", nullable: false),
                    UsageQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rel_MachineBOM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rel_MachineBOM_Mdm_Machines_MachineID",
                        column: x => x.MachineID,
                        principalTable: "Mdm_Machines",
                        principalColumn: "MachineID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rel_MachineBOM_Mdm_Materials_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Mdm_Materials",
                        principalColumn: "MaterialID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inv_CurrentStock",
                columns: table => new
                {
                    StockID = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaterialID = table.Column<int>(type: "INTEGER", nullable: false),
                    WarehouseID = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BinLocation = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inv_CurrentStock", x => x.StockID);
                    table.ForeignKey(
                        name: "FK_Inv_CurrentStock_Mdm_Materials_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Mdm_Materials",
                        principalColumn: "MaterialID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inv_CurrentStock_Mdm_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Mdm_Warehouses",
                        principalColumn: "WarehouseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inv_CurrentStock_MaterialID",
                table: "Inv_CurrentStock",
                column: "MaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_Inv_CurrentStock_WarehouseID",
                table: "Inv_CurrentStock",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_Inv_Transactions_MaterialID",
                table: "Inv_Transactions",
                column: "MaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_Rel_MachineBOM_MachineID",
                table: "Rel_MachineBOM",
                column: "MachineID");

            migrationBuilder.CreateIndex(
                name: "IX_Rel_MachineBOM_MaterialID",
                table: "Rel_MachineBOM",
                column: "MaterialID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inv_CurrentStock");

            migrationBuilder.DropTable(
                name: "Inv_Transactions");

            migrationBuilder.DropTable(
                name: "Rel_MachineBOM");

            migrationBuilder.DropTable(
                name: "Mdm_Warehouses");

            migrationBuilder.DropTable(
                name: "Mdm_Machines");

            migrationBuilder.DropTable(
                name: "Mdm_Materials");
        }
    }
}

