using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdateAllTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "IsSeen",
            table: "Notifications",
            newName: "IsRead");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "IsRead",
            table: "Notifications",
            newName: "IsSeen");
    }
}
