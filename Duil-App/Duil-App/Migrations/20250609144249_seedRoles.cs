﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Duil_App.Migrations
{
    /// <inheritdoc />
    public partial class seedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
                    { Guid.NewGuid().ToString(), "Cliente", "CLIENTE", Guid.NewGuid().ToString() },
                    { Guid.NewGuid().ToString(), "Funcionario", "FUNCIONARIO", Guid.NewGuid().ToString() },
                    { Guid.NewGuid().ToString(), "Admin", "ADMIN", Guid.NewGuid().ToString() }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Name",
                keyValues: new object[] { "Cliente", "Funcionario", "Admin" });
        }
    }
}