using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddPaymentCouponTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "CoverImageFileName",
            table: "Coupons",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "CoverImageUrl",
            table: "Coupons",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateTable(
            name: "PaymentCoupons",
            columns: table => new
            {
                PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CouponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PaymentCoupons");

        migrationBuilder.DropColumn(
            name: "CoverImageFileName",
            table: "Coupons");

        migrationBuilder.DropColumn(
            name: "CoverImageUrl",
            table: "Coupons");
    }
}
