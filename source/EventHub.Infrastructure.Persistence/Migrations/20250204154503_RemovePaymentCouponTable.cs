using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class RemovePaymentCouponTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_AspNetUsers_UserId",
            table: "Tickets");

        migrationBuilder.DropTable(
            name: "PaymentCoupons");

        migrationBuilder.DropIndex(
            name: "IX_Tickets_UserId",
            table: "Tickets");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "Tickets");

        migrationBuilder.AlterColumn<string>(
            name: "TicketNo",
            table: "Tickets",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50);

        migrationBuilder.AddColumn<Guid>(
            name: "CouponId",
            table: "Payments",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        migrationBuilder.CreateIndex(
            name: "IX_Tickets_AuthorId",
            table: "Tickets",
            column: "AuthorId");

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

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_AspNetUsers_AuthorId",
            table: "Tickets",
            column: "AuthorId",
            principalTable: "AspNetUsers",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Payments_Coupons_CouponId",
            table: "Payments");

        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_AspNetUsers_AuthorId",
            table: "Tickets");

        migrationBuilder.DropIndex(
            name: "IX_Tickets_AuthorId",
            table: "Tickets");

        migrationBuilder.DropIndex(
            name: "IX_Payments_CouponId",
            table: "Payments");

        migrationBuilder.DropColumn(
            name: "CouponId",
            table: "Payments");

        migrationBuilder.AlterColumn<string>(
            name: "TicketNo",
            table: "Tickets",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddColumn<Guid>(
            name: "UserId",
            table: "Tickets",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

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
            name: "IX_Tickets_UserId",
            table: "Tickets",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_PaymentCoupons_CouponId",
            table: "PaymentCoupons",
            column: "CouponId");

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_AspNetUsers_UserId",
            table: "Tickets",
            column: "UserId",
            principalTable: "AspNetUsers",
            principalColumn: "Id");
    }
}
