using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class RemovePaymentCouponTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PaymentCoupons");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PaymentCoupons",
            columns: table => new
            {
                PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CouponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PaymentCoupons", x => new { x.PaymentId, x.CouponId });
                table.ForeignKey(
                    name: "FK_PaymentCoupons_Coupons_CouponId",
                    column: x => x.CouponId,
                    principalTable: "Coupons",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_PaymentCoupons_Payments_PaymentId",
                    column: x => x.PaymentId,
                    principalTable: "Payments",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_PaymentCoupons_CouponId",
            table: "PaymentCoupons",
            column: "CouponId");
    }
}
