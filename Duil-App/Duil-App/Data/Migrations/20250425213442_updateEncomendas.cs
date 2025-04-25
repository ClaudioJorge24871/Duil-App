using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duil_App.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateEncomendas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encomendas_Clientes_ClienteNif",
                table: "Encomendas");

            migrationBuilder.DropIndex(
                name: "IX_Encomendas_ClienteNif",
                table: "Encomendas");

            migrationBuilder.DropColumn(
                name: "ClienteNif",
                table: "Encomendas");

            migrationBuilder.AlterColumn<string>(
                name: "Designacao",
                table: "Pecas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClienteId",
                table: "Encomendas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Empresas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Encomendas_ClienteId",
                table: "Encomendas",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Encomendas_Clientes_ClienteId",
                table: "Encomendas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Nif",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encomendas_Clientes_ClienteId",
                table: "Encomendas");

            migrationBuilder.DropIndex(
                name: "IX_Encomendas_ClienteId",
                table: "Encomendas");

            migrationBuilder.AlterColumn<string>(
                name: "Designacao",
                table: "Pecas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "Encomendas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ClienteNif",
                table: "Encomendas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Empresas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Encomendas_ClienteNif",
                table: "Encomendas",
                column: "ClienteNif");

            migrationBuilder.AddForeignKey(
                name: "FK_Encomendas_Clientes_ClienteNif",
                table: "Encomendas",
                column: "ClienteNif",
                principalTable: "Clientes",
                principalColumn: "Nif",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
