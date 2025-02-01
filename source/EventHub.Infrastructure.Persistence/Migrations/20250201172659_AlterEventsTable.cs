using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AlterEventsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "NumberOfShares",
            table: "Events");

        migrationBuilder.DropColumn(
            name: "Promotion",
            table: "Events");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "NumberOfShares",
            table: "Events",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<double>(
            name: "Promotion",
            table: "Events",
            type: "float",
            nullable: true);
    }
}
