using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdatePaymentDiscountTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<long>(
            name: "Discount",
            table: "Payments",
            type: "bigint",
            nullable: false,
            oldClrType: typeof(double),
            oldType: "float");

        migrationBuilder.AlterColumn<int>(
            name: "PercentValue",
            table: "Coupons",
            type: "int",
            nullable: false,
            oldClrType: typeof(float),
            oldType: "real");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<double>(
            name: "Discount",
            table: "Payments",
            type: "float",
            nullable: false,
            oldClrType: typeof(long),
            oldType: "bigint");

        migrationBuilder.AlterColumn<float>(
            name: "PercentValue",
            table: "Coupons",
            type: "real",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");
    }
}
