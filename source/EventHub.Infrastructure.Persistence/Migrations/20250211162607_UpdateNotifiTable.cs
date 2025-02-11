using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdateNotifiTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "IsRead",
            table: "Notifications",
            type: "int",
            nullable: false,
            oldClrType: typeof(bool),
            oldType: "bit");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<bool>(
            name: "IsRead",
            table: "Notifications",
            type: "bit",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");
    }
}
