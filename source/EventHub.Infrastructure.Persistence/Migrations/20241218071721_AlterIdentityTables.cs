using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AlterIdentityTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "UserId",
            table: "AspNetRoles",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_AspNetRoles_UserId",
            table: "AspNetRoles",
            column: "UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_AspNetRoles_AspNetUsers_UserId",
            table: "AspNetRoles",
            column: "UserId",
            principalTable: "AspNetUsers",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_AspNetRoles_AspNetUsers_UserId",
            table: "AspNetRoles");

        migrationBuilder.DropIndex(
            name: "IX_AspNetRoles_UserId",
            table: "AspNetRoles");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "AspNetRoles");
    }
}
