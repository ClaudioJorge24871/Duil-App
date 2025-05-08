using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duil_App.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedPecasFabricaTypeValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pecas_Fabricas_FabricaNif",
                table: "Pecas");

            migrationBuilder.DropIndex(
                name: "IX_Pecas_FabricaNif",
                table: "Pecas");

            migrationBuilder.DropColumn(
                name: "FabricaNif",
                table: "Pecas");

            migrationBuilder.AlterColumn<string>(
                name: "FabricaId",
                table: "Pecas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdLadoCliente",
                table: "Encomendas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Pecas_FabricaId",
                table: "Pecas",
                column: "FabricaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pecas_Fabricas_FabricaId",
                table: "Pecas",
                column: "FabricaId",
                principalTable: "Fabricas",
                principalColumn: "Nif",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pecas_Fabricas_FabricaId",
                table: "Pecas");

            migrationBuilder.DropIndex(
                name: "IX_Pecas_FabricaId",
                table: "Pecas");

            migrationBuilder.AlterColumn<int>(
                name: "FabricaId",
                table: "Pecas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "FabricaNif",
                table: "Pecas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdLadoCliente",
                table: "Encomendas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pecas_FabricaNif",
                table: "Pecas",
                column: "FabricaNif");

            migrationBuilder.AddForeignKey(
                name: "FK_Pecas_Fabricas_FabricaNif",
                table: "Pecas",
                column: "FabricaNif",
                principalTable: "Fabricas",
                principalColumn: "Nif");
        }
    }
}
