using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdateMessageFieldTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "ReceiverId",
            table: "Messages",
            type: "uniqueidentifier",
            nullable: true,
            defaultValue: null);

        migrationBuilder.CreateIndex(
            name: "IX_Messages_ReceiverId",
            table: "Messages",
            column: "ReceiverId");

        migrationBuilder.AddForeignKey(
            name: "FK_Messages_AspNetUsers_ReceiverId",
            table: "Messages",
            column: "ReceiverId",
            principalTable: "AspNetUsers",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Messages_AspNetUsers_ReceiverId",
            table: "Messages");

        migrationBuilder.DropIndex(
            name: "IX_Messages_ReceiverId",
            table: "Messages");

        migrationBuilder.DropColumn(
            name: "ReceiverId",
            table: "Messages");
    }
}
