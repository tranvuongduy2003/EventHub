using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdateNotificationAddFiledTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Notifications_Invitations_InvitationId",
            table: "Notifications");

        migrationBuilder.DropForeignKey(
            name: "FK_Notifications_Payments_PaymentId",
            table: "Notifications");

        migrationBuilder.DropForeignKey(
            name: "FK_Notifications_UserFollowers_UserFollowerId",
            table: "Notifications");

        migrationBuilder.AddForeignKey(
            name: "FK_Notifications_Invitations_InvitationId",
            table: "Notifications",
            column: "InvitationId",
            principalTable: "Invitations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Notifications_Payments_PaymentId",
            table: "Notifications",
            column: "PaymentId",
            principalTable: "Payments",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Notifications_UserFollowers_UserFollowerId",
            table: "Notifications",
            column: "UserFollowerId",
            principalTable: "UserFollowers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Notifications_Invitations_InvitationId",
            table: "Notifications");

        migrationBuilder.DropForeignKey(
            name: "FK_Notifications_Payments_PaymentId",
            table: "Notifications");

        migrationBuilder.DropForeignKey(
            name: "FK_Notifications_UserFollowers_UserFollowerId",
            table: "Notifications");

        migrationBuilder.AddForeignKey(
            name: "FK_Notifications_Invitations_InvitationId",
            table: "Notifications",
            column: "InvitationId",
            principalTable: "Invitations",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Notifications_Payments_PaymentId",
            table: "Notifications",
            column: "PaymentId",
            principalTable: "Payments",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Notifications_UserFollowers_UserFollowerId",
            table: "Notifications",
            column: "UserFollowerId",
            principalTable: "UserFollowers",
            principalColumn: "Id");
    }
}
