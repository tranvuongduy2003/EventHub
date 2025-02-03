using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdateTicketPaymentTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_AspNetUsers_UserId",
            table: "Tickets");

        migrationBuilder.DropIndex(
            name: "IX_Tickets_UserId",
            table: "Tickets");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "Tickets");

        migrationBuilder.AlterColumn<string>(
            name: "TicketNo",
            table: "Tickets",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50);

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_AuthorId",
            table: "Tickets",
            column: "AuthorId");

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_AspNetUsers_AuthorId",
            table: "Tickets",
            column: "AuthorId",
            principalTable: "AspNetUsers",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_AspNetUsers_AuthorId",
            table: "Tickets");

        migrationBuilder.DropIndex(
            name: "IX_Tickets_AuthorId",
            table: "Tickets");

        migrationBuilder.AlterColumn<string>(
            name: "TicketNo",
            table: "Tickets",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddColumn<Guid>(
            name: "UserId",
            table: "Tickets",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_UserId",
            table: "Tickets",
            column: "UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_AspNetUsers_UserId",
            table: "Tickets",
            column: "UserId",
            principalTable: "AspNetUsers",
            principalColumn: "Id");
    }
}
