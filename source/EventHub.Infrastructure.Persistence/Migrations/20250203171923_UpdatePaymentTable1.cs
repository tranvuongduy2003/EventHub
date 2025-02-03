using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdatePaymentTable1 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "CouponId",
            table: "Payments",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        migrationBuilder.CreateIndex(
            name: "IX_Payments_CouponId",
            table: "Payments",
            column: "CouponId");

        migrationBuilder.AddForeignKey(
            name: "FK_Payments_Coupons_CouponId",
            table: "Payments",
            column: "CouponId",
            principalTable: "Coupons",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Payments_Coupons_CouponId",
            table: "Payments");

        migrationBuilder.DropIndex(
            name: "IX_Payments_CouponId",
            table: "Payments");

        migrationBuilder.DropColumn(
            name: "CouponId",
            table: "Payments");
    }
}
