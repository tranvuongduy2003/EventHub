using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class RemoveUnusedTables : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Payments_UserPaymentMethods_UserPaymentMethodId",
            table: "Payments");

        migrationBuilder.DropTable(
            name: "LabelInEvents");

        migrationBuilder.DropTable(
            name: "LabelInUsers");

        migrationBuilder.DropTable(
            name: "PermissionAggregates");

        migrationBuilder.DropTable(
            name: "UserPaymentMethods");

        migrationBuilder.DropTable(
            name: "Labels");

        migrationBuilder.DropTable(
            name: "PaymentMethods");

        migrationBuilder.DropIndex(
            name: "IX_Payments_UserPaymentMethodId",
            table: "Payments");

        migrationBuilder.DropColumn(
            name: "UserPaymentMethodId",
            table: "Payments");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "UserPaymentMethodId",
            table: "Payments",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        migrationBuilder.CreateTable(
            name: "Labels",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Labels", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PaymentMethods",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                MethodLogoFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                MethodLogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                MethodName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PaymentMethods", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PermissionAggregates",
            columns: table => new
            {
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
            });

        migrationBuilder.CreateTable(
            name: "LabelInEvents",
            columns: table => new
            {
                LabelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LabelInEvents", x => new { x.LabelId, x.EventId });
                table.ForeignKey(
                    name: "FK_LabelInEvents_Events_EventId",
                    column: x => x.EventId,
                    principalTable: "Events",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_LabelInEvents_Labels_LabelId",
                    column: x => x.LabelId,
                    principalTable: "Labels",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "LabelInUsers",
            columns: table => new
            {
                LabelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LabelInUsers", x => new { x.LabelId, x.AuthorId });
                table.ForeignKey(
                    name: "FK_LabelInUsers_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_LabelInUsers_Labels_LabelId",
                    column: x => x.LabelId,
                    principalTable: "Labels",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "UserPaymentMethods",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CheckoutContent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                PaymentAccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                PaymentAccountQrCodeFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                PaymentAccountQrCodeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserPaymentMethods", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserPaymentMethods_AspNetUsers_AuthorId",
                    column: x => x.AuthorId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserPaymentMethods_PaymentMethods_PaymentMethodId",
                    column: x => x.PaymentMethodId,
                    principalTable: "PaymentMethods",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Payments_UserPaymentMethodId",
            table: "Payments",
            column: "UserPaymentMethodId");

        migrationBuilder.CreateIndex(
            name: "IX_LabelInEvents_EventId",
            table: "LabelInEvents",
            column: "EventId");

        migrationBuilder.CreateIndex(
            name: "IX_LabelInUsers_UserId",
            table: "LabelInUsers",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserPaymentMethods_AuthorId",
            table: "UserPaymentMethods",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_UserPaymentMethods_PaymentMethodId",
            table: "UserPaymentMethods",
            column: "PaymentMethodId");

        migrationBuilder.AddForeignKey(
            name: "FK_Payments_UserPaymentMethods_UserPaymentMethodId",
            table: "Payments",
            column: "UserPaymentMethodId",
            principalTable: "UserPaymentMethods",
            principalColumn: "Id");
    }
}
