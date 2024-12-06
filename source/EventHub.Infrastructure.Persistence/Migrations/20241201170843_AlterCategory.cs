using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AlterCategory : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Categories_Events_EventId",
            table: "Categories");

        migrationBuilder.DropForeignKey(
            name: "FK_Conversations_Events_EventId",
            table: "Conversations");

        migrationBuilder.DropForeignKey(
            name: "FK_EmailContents_Events_EventId",
            table: "EmailContents");

        migrationBuilder.DropForeignKey(
            name: "FK_EventCategories_Events_EventId",
            table: "EventCategories");

        migrationBuilder.DropForeignKey(
            name: "FK_Events_AspNetUsers_AuthorId",
            table: "Events");

        migrationBuilder.DropForeignKey(
            name: "FK_EventSubImages_Events_EventId",
            table: "EventSubImages");

        migrationBuilder.DropForeignKey(
            name: "FK_FavouriteEvents_Events_EventId",
            table: "FavouriteEvents");

        migrationBuilder.DropForeignKey(
            name: "FK_Invitations_Events_EventId",
            table: "Invitations");

        migrationBuilder.DropForeignKey(
            name: "FK_LabelInEvents_Events_EventId",
            table: "LabelInEvents");

        migrationBuilder.DropForeignKey(
            name: "FK_Messages_Events_EventId",
            table: "Messages");

        migrationBuilder.DropForeignKey(
            name: "FK_PaymentItems_Events_EventId",
            table: "PaymentItems");

        migrationBuilder.DropForeignKey(
            name: "FK_Payments_Events_EventId",
            table: "Payments");

        migrationBuilder.DropForeignKey(
            name: "FK_Reasons_Events_EventId",
            table: "Reasons");

        migrationBuilder.DropForeignKey(
            name: "FK_Reviews_Events_EventId",
            table: "Reviews");

        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_Events_EventId",
            table: "Tickets");

        migrationBuilder.DropForeignKey(
            name: "FK_TicketTypes_Events_EventId",
            table: "TicketTypes");

        migrationBuilder.DropPrimaryKey(
            name: "PK_LabelInUsers",
            table: "LabelInUsers");

        migrationBuilder.DropPrimaryKey(
            name: "PK_FavouriteEvents",
            table: "FavouriteEvents");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Events",
            table: "Events");

        migrationBuilder.RenameTable(
            name: "Events",
            newName: "events");

        migrationBuilder.RenameIndex(
            name: "IX_Events_AuthorId",
            table: "events",
            newName: "IX_events_AuthorId");

        migrationBuilder.AddColumn<Guid>(
            name: "AuthorId",
            table: "Tickets",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        migrationBuilder.AddColumn<Guid>(
            name: "AuthorId",
            table: "LabelInUsers",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        migrationBuilder.AddColumn<Guid>(
            name: "AuthorId",
            table: "FavouriteEvents",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        migrationBuilder.AddColumn<Guid>(
            name: "AuthorId",
            table: "Conversations",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        migrationBuilder.AlterColumn<string>(
            name: "IconImageUrl",
            table: "Categories",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(255)",
            oldMaxLength: 255);

        migrationBuilder.AddPrimaryKey(
            name: "PK_LabelInUsers",
            table: "LabelInUsers",
            columns: ["LabelId", "AuthorId"]);

        migrationBuilder.AddPrimaryKey(
            name: "PK_FavouriteEvents",
            table: "FavouriteEvents",
            columns: ["AuthorId", "EventId"]);

        migrationBuilder.AddPrimaryKey(
            name: "PK_events",
            table: "events",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_FavouriteEvents_UserId",
            table: "FavouriteEvents",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_Conversations_LastMessageId",
            table: "Conversations",
            column: "LastMessageId");

        migrationBuilder.AddForeignKey(
            name: "FK_Categories_events_EventId",
            table: "Categories",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Conversations_Messages_LastMessageId",
            table: "Conversations",
            column: "LastMessageId",
            principalTable: "Messages",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Conversations_events_EventId",
            table: "Conversations",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_EmailContents_events_EventId",
            table: "EmailContents",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_EventCategories_events_EventId",
            table: "EventCategories",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_events_AspNetUsers_AuthorId",
            table: "events",
            column: "AuthorId",
            principalTable: "AspNetUsers",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_EventSubImages_events_EventId",
            table: "EventSubImages",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_FavouriteEvents_events_EventId",
            table: "FavouriteEvents",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Invitations_events_EventId",
            table: "Invitations",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_LabelInEvents_events_EventId",
            table: "LabelInEvents",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Messages_events_EventId",
            table: "Messages",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_PaymentItems_events_EventId",
            table: "PaymentItems",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Payments_events_EventId",
            table: "Payments",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Reasons_events_EventId",
            table: "Reasons",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Reviews_events_EventId",
            table: "Reviews",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_events_EventId",
            table: "Tickets",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_TicketTypes_events_EventId",
            table: "TicketTypes",
            column: "EventId",
            principalTable: "events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Categories_events_EventId",
            table: "Categories");

        migrationBuilder.DropForeignKey(
            name: "FK_Conversations_Messages_LastMessageId",
            table: "Conversations");

        migrationBuilder.DropForeignKey(
            name: "FK_Conversations_events_EventId",
            table: "Conversations");

        migrationBuilder.DropForeignKey(
            name: "FK_EmailContents_events_EventId",
            table: "EmailContents");

        migrationBuilder.DropForeignKey(
            name: "FK_EventCategories_events_EventId",
            table: "EventCategories");

        migrationBuilder.DropForeignKey(
            name: "FK_events_AspNetUsers_AuthorId",
            table: "events");

        migrationBuilder.DropForeignKey(
            name: "FK_EventSubImages_events_EventId",
            table: "EventSubImages");

        migrationBuilder.DropForeignKey(
            name: "FK_FavouriteEvents_events_EventId",
            table: "FavouriteEvents");

        migrationBuilder.DropForeignKey(
            name: "FK_Invitations_events_EventId",
            table: "Invitations");

        migrationBuilder.DropForeignKey(
            name: "FK_LabelInEvents_events_EventId",
            table: "LabelInEvents");

        migrationBuilder.DropForeignKey(
            name: "FK_Messages_events_EventId",
            table: "Messages");

        migrationBuilder.DropForeignKey(
            name: "FK_PaymentItems_events_EventId",
            table: "PaymentItems");

        migrationBuilder.DropForeignKey(
            name: "FK_Payments_events_EventId",
            table: "Payments");

        migrationBuilder.DropForeignKey(
            name: "FK_Reasons_events_EventId",
            table: "Reasons");

        migrationBuilder.DropForeignKey(
            name: "FK_Reviews_events_EventId",
            table: "Reviews");

        migrationBuilder.DropForeignKey(
            name: "FK_Tickets_events_EventId",
            table: "Tickets");

        migrationBuilder.DropForeignKey(
            name: "FK_TicketTypes_events_EventId",
            table: "TicketTypes");

        migrationBuilder.DropPrimaryKey(
            name: "PK_LabelInUsers",
            table: "LabelInUsers");

        migrationBuilder.DropPrimaryKey(
            name: "PK_FavouriteEvents",
            table: "FavouriteEvents");

        migrationBuilder.DropIndex(
            name: "IX_FavouriteEvents_UserId",
            table: "FavouriteEvents");

        migrationBuilder.DropPrimaryKey(
            name: "PK_events",
            table: "events");

        migrationBuilder.DropIndex(
            name: "IX_Conversations_LastMessageId",
            table: "Conversations");

        migrationBuilder.DropColumn(
            name: "AuthorId",
            table: "Tickets");

        migrationBuilder.DropColumn(
            name: "AuthorId",
            table: "LabelInUsers");

        migrationBuilder.DropColumn(
            name: "AuthorId",
            table: "FavouriteEvents");

        migrationBuilder.DropColumn(
            name: "AuthorId",
            table: "Conversations");

        migrationBuilder.RenameTable(
            name: "events",
            newName: "Events");

        migrationBuilder.RenameIndex(
            name: "IX_events_AuthorId",
            table: "Events",
            newName: "IX_Events_AuthorId");

        migrationBuilder.AlterColumn<string>(
            name: "IconImageUrl",
            table: "Categories",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddPrimaryKey(
            name: "PK_LabelInUsers",
            table: "LabelInUsers",
            columns: ["LabelId", "UserId"]);

        migrationBuilder.AddPrimaryKey(
            name: "PK_FavouriteEvents",
            table: "FavouriteEvents",
            columns: ["UserId", "EventId"]);

        migrationBuilder.AddPrimaryKey(
            name: "PK_Events",
            table: "Events",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Categories_Events_EventId",
            table: "Categories",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Conversations_Events_EventId",
            table: "Conversations",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_EmailContents_Events_EventId",
            table: "EmailContents",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_EventCategories_Events_EventId",
            table: "EventCategories",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Events_AspNetUsers_AuthorId",
            table: "Events",
            column: "AuthorId",
            principalTable: "AspNetUsers",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_EventSubImages_Events_EventId",
            table: "EventSubImages",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_FavouriteEvents_Events_EventId",
            table: "FavouriteEvents",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Invitations_Events_EventId",
            table: "Invitations",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_LabelInEvents_Events_EventId",
            table: "LabelInEvents",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Messages_Events_EventId",
            table: "Messages",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_PaymentItems_Events_EventId",
            table: "PaymentItems",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Payments_Events_EventId",
            table: "Payments",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Reasons_Events_EventId",
            table: "Reasons",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Reviews_Events_EventId",
            table: "Reviews",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Tickets_Events_EventId",
            table: "Tickets",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_TicketTypes_Events_EventId",
            table: "TicketTypes",
            column: "EventId",
            principalTable: "Events",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}

