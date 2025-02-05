using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdatePaymentCouponTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "CouponId",
            table: "Payments",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "CouponId",
            table: "Payments",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid(),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);
    }
}
