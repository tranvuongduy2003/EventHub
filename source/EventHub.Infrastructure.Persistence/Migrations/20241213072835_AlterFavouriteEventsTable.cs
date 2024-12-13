using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AlterFavouriteEventsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_FavouriteEvents",
            table: "FavouriteEvents");

        migrationBuilder.DropIndex(
            name: "IX_FavouriteEvents_UserId",
            table: "FavouriteEvents");

        migrationBuilder.DropColumn(
            name: "AuthorId",
            table: "FavouriteEvents");

        migrationBuilder.AddPrimaryKey(
            name: "PK_FavouriteEvents",
            table: "FavouriteEvents",
            columns: [ "UserId", "EventId" ]);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_FavouriteEvents",
            table: "FavouriteEvents");

        migrationBuilder.AddColumn<Guid>(
            name: "AuthorId",
            table: "FavouriteEvents",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid());

        migrationBuilder.AddPrimaryKey(
            name: "PK_FavouriteEvents",
            table: "FavouriteEvents",
            columns: ["AuthorId", "EventId"]);

        migrationBuilder.CreateIndex(
            name: "IX_FavouriteEvents_UserId",
            table: "FavouriteEvents",
            column: "UserId");
    }
}

