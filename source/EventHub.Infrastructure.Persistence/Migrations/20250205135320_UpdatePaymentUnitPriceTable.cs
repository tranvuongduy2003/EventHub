using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdatePaymentUnitPriceTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<long>(
            name: "UnitPrice",
            table: "PaymentItems",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "UnitPrice",
            table: "PaymentItems");
    }
}
