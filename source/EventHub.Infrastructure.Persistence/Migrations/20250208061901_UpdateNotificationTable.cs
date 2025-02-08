using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class UpdateNotificationTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_UserFollowers",
            table: "UserFollowers");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Invitations",
            table: "Invitations");

        migrationBuilder.AddColumn<Guid>(
            name: "Id",
            table: "UserFollowers",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        migrationBuilder.AddColumn<Guid>(
            name: "InvitationId",
            table: "Notifications",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsSeen",
            table: "Notifications",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<Guid>(
            name: "PaymentId",
            table: "Notifications",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "UserFollowerId",
            table: "Notifications",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "Id",
            table: "Invitations",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        migrationBuilder.AddPrimaryKey(
            name: "PK_UserFollowers",
            table: "UserFollowers",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Invitations",
            table: "Invitations",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_UserFollowers_FollowerId",
            table: "UserFollowers",
            column: "FollowerId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_InvitationId",
            table: "Notifications",
            column: "InvitationId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_PaymentId",
            table: "Notifications",
            column: "PaymentId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_UserFollowerId",
            table: "Notifications",
            column: "UserFollowerId");

        migrationBuilder.CreateIndex(
            name: "IX_Invitations_InviterId",
            table: "Invitations",
            column: "InviterId");

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

    private static readonly string[] columns = new[] { "FollowerId", "FollowedId" };
    private static readonly string[] columnsArray = new[] { "InviterId", "InvitedId", "EventId" };

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

        migrationBuilder.DropPrimaryKey(
            name: "PK_UserFollowers",
            table: "UserFollowers");

        migrationBuilder.DropIndex(
            name: "IX_UserFollowers_FollowerId",
            table: "UserFollowers");

        migrationBuilder.DropIndex(
            name: "IX_Notifications_InvitationId",
            table: "Notifications");

        migrationBuilder.DropIndex(
            name: "IX_Notifications_PaymentId",
            table: "Notifications");

        migrationBuilder.DropIndex(
            name: "IX_Notifications_UserFollowerId",
            table: "Notifications");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Invitations",
            table: "Invitations");

        migrationBuilder.DropIndex(
            name: "IX_Invitations_InviterId",
            table: "Invitations");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "UserFollowers");

        migrationBuilder.DropColumn(
            name: "InvitationId",
            table: "Notifications");

        migrationBuilder.DropColumn(
            name: "IsSeen",
            table: "Notifications");

        migrationBuilder.DropColumn(
            name: "PaymentId",
            table: "Notifications");

        migrationBuilder.DropColumn(
            name: "UserFollowerId",
            table: "Notifications");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "Invitations");

        migrationBuilder.AddPrimaryKey(
            name: "PK_UserFollowers",
            table: "UserFollowers",
            columns: columns);

        migrationBuilder.AddPrimaryKey(
            name: "PK_Invitations",
            table: "Invitations",
            columns: columnsArray);
    }
}
