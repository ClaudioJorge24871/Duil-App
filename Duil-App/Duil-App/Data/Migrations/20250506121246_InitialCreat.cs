using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duil_App.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Nif = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CodPostal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Nif);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Nif = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MoradaCarga = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Nif);
                    table.ForeignKey(
                        name: "FK_Clientes_Empresas_Nif",
                        column: x => x.Nif,
                        principalTable: "Empresas",
                        principalColumn: "Nif",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fabricas",
                columns: table => new
                {
                    Nif = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MoradaDescarga = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fabricas", x => x.Nif);
                    table.ForeignKey(
                        name: "FK_Fabricas_Empresas_Nif",
                        column: x => x.Nif,
                        principalTable: "Empresas",
                        principalColumn: "Nif",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Encomendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdLadoCliente = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrecoUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantidadeTotal = table.Column<int>(type: "int", nullable: false),
                    Transportadora = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encomendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Encomendas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Nif",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pecas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Referencia = table.Column<int>(type: "int", nullable: false),
                    Designacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PrecoUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FabricaId = table.Column<int>(type: "int", nullable: false),
                    FabricaNif = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pecas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pecas_Fabricas_FabricaNif",
                        column: x => x.FabricaNif,
                        principalTable: "Fabricas",
                        principalColumn: "Nif");
                });

            migrationBuilder.CreateTable(
                name: "LinhasEncomendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EncomendaId = table.Column<int>(type: "int", nullable: false),
                    PecaId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinhasEncomendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinhasEncomendas_Encomendas_EncomendaId",
                        column: x => x.EncomendaId,
                        principalTable: "Encomendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinhasEncomendas_Pecas_PecaId",
                        column: x => x.PecaId,
                        principalTable: "Pecas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Encomendas_ClienteId",
                table: "Encomendas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_LinhasEncomendas_EncomendaId",
                table: "LinhasEncomendas",
                column: "EncomendaId");

            migrationBuilder.CreateIndex(
                name: "IX_LinhasEncomendas_PecaId",
                table: "LinhasEncomendas",
                column: "PecaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pecas_FabricaNif",
                table: "Pecas",
                column: "FabricaNif");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinhasEncomendas");

            migrationBuilder.DropTable(
                name: "Encomendas");

            migrationBuilder.DropTable(
                name: "Pecas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Fabricas");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
