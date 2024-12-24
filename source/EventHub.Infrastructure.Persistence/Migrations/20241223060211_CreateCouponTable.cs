using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class CreateCouponTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EmailLoggers");

        migrationBuilder.CreateTable(
            name: "Coupons",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                MinQuantity = table.Column<int>(type: "int", nullable: false),
                MinPrice = table.Column<long>(type: "bigint", nullable: false),
                PercentValue = table.Column<float>(type: "real", nullable: false),
                ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Coupons", x => x.Id);
                table.ForeignKey(
                    name: "FK_Coupons_AspNetUsers_AuthorId",
                    column: x => x.AuthorId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "EventCoupons",
            columns: table => new
            {
                CouponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EventCoupons", x => new { x.CouponId, x.EventId });
                table.ForeignKey(
                    name: "FK_EventCoupons_Coupons_CouponId",
                    column: x => x.CouponId,
                    principalTable: "Coupons",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_EventCoupons_Events_EventId",
                    column: x => x.EventId,
                    principalTable: "Events",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Coupons_AuthorId",
            table: "Coupons",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_EventCoupons_EventId",
            table: "EventCoupons",
            column: "EventId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EventCoupons");

        migrationBuilder.DropTable(
            name: "Coupons");

        migrationBuilder.CreateTable(
            name: "EmailLoggers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EmailContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                ReceiverEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                SentEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmailLoggers", x => x.Id);
                table.ForeignKey(
                    name: "FK_EmailLoggers_EmailContents_EmailContentId",
                    column: x => x.EmailContentId,
                    principalTable: "EmailContents",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_EmailLoggers_EmailContentId",
            table: "EmailLoggers",
            column: "EmailContentId");
    }
}
