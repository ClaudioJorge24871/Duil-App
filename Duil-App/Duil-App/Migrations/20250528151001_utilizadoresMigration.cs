using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duil_App.Migrations
{
    /// <inheritdoc />
    public partial class utilizadoresMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UtilizadoresId",
                table: "Encomendas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CodPostal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NIF = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Encomendas_UtilizadoresId",
                table: "Encomendas",
                column: "UtilizadoresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Encomendas_Utilizadores_UtilizadoresId",
                table: "Encomendas",
                column: "UtilizadoresId",
                principalTable: "Utilizadores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encomendas_Utilizadores_UtilizadoresId",
                table: "Encomendas");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Encomendas_UtilizadoresId",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "UtilizadoresId",
                table: "Encomendas");
        }
    }
}
