using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duil_App.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinhaEncomenda_Encomendas_EncomendaId",
                table: "LinhaEncomenda");

            migrationBuilder.DropForeignKey(
                name: "FK_LinhaEncomenda_Pecas_PecaId",
                table: "LinhaEncomenda");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinhaEncomenda",
                table: "LinhaEncomenda");

            migrationBuilder.RenameTable(
                name: "LinhaEncomenda",
                newName: "LinhasEncomendas");

            migrationBuilder.RenameIndex(
                name: "IX_LinhaEncomenda_PecaId",
                table: "LinhasEncomendas",
                newName: "IX_LinhasEncomendas_PecaId");

            migrationBuilder.RenameIndex(
                name: "IX_LinhaEncomenda_EncomendaId",
                table: "LinhasEncomendas",
                newName: "IX_LinhasEncomendas_EncomendaId");

            migrationBuilder.AlterColumn<string>(
                name: "MoradaDescarga",
                table: "Fabricas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Transportadora",
                table: "Encomendas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Pais",
                table: "Empresas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Empresas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Morada",
                table: "Empresas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinhasEncomendas",
                table: "LinhasEncomendas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LinhasEncomendas_Encomendas_EncomendaId",
                table: "LinhasEncomendas",
                column: "EncomendaId",
                principalTable: "Encomendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LinhasEncomendas_Pecas_PecaId",
                table: "LinhasEncomendas",
                column: "PecaId",
                principalTable: "Pecas",
                principalColumn: "Referencia",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinhasEncomendas_Encomendas_EncomendaId",
                table: "LinhasEncomendas");

            migrationBuilder.DropForeignKey(
                name: "FK_LinhasEncomendas_Pecas_PecaId",
                table: "LinhasEncomendas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LinhasEncomendas",
                table: "LinhasEncomendas");

            migrationBuilder.RenameTable(
                name: "LinhasEncomendas",
                newName: "LinhaEncomenda");

            migrationBuilder.RenameIndex(
                name: "IX_LinhasEncomendas_PecaId",
                table: "LinhaEncomenda",
                newName: "IX_LinhaEncomenda_PecaId");

            migrationBuilder.RenameIndex(
                name: "IX_LinhasEncomendas_EncomendaId",
                table: "LinhaEncomenda",
                newName: "IX_LinhaEncomenda_EncomendaId");

            migrationBuilder.AlterColumn<string>(
                name: "MoradaDescarga",
                table: "Fabricas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Transportadora",
                table: "Encomendas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Pais",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Morada",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LinhaEncomenda",
                table: "LinhaEncomenda",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LinhaEncomenda_Encomendas_EncomendaId",
                table: "LinhaEncomenda",
                column: "EncomendaId",
                principalTable: "Encomendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LinhaEncomenda_Pecas_PecaId",
                table: "LinhaEncomenda",
                column: "PecaId",
                principalTable: "Pecas",
                principalColumn: "Referencia",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
