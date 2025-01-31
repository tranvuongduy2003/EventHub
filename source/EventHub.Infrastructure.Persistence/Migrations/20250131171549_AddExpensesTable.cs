using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHub.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddExpensesTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PaymentSessionId",
            table: "Payments");

        migrationBuilder.DropColumn(
            name: "Discount",
            table: "PaymentItems");

        migrationBuilder.AddColumn<string>(
            name: "PaymentIntentId",
            table: "Payments",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PaymentMethod",
            table: "Payments",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SessionId",
            table: "Payments",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "EventId",
            table: "PaymentItems",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: Guid.NewGuid(),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.CreateTable(
            name: "Expenses",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                Total = table.Column<long>(type: "bigint", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Expenses", x => x.Id);
                table.ForeignKey(
                    name: "FK_Expenses_Events_EventId",
                    column: x => x.EventId,
                    principalTable: "Events",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SubExpenses",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                Price = table.Column<long>(type: "bigint", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SubExpenses", x => x.Id);
                table.ForeignKey(
                    name: "FK_SubExpenses_Expenses_ExpenseId",
                    column: x => x.ExpenseId,
                    principalTable: "Expenses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Expenses_EventId",
            table: "Expenses",
            column: "EventId");

        migrationBuilder.CreateIndex(
            name: "IX_SubExpenses_ExpenseId",
            table: "SubExpenses",
            column: "ExpenseId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SubExpenses");

        migrationBuilder.DropTable(
            name: "Expenses");

        migrationBuilder.DropColumn(
            name: "PaymentIntentId",
            table: "Payments");

        migrationBuilder.DropColumn(
            name: "PaymentMethod",
            table: "Payments");

        migrationBuilder.DropColumn(
            name: "SessionId",
            table: "Payments");

        migrationBuilder.AddColumn<Guid>(
            name: "PaymentSessionId",
            table: "Payments",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "EventId",
            table: "PaymentItems",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddColumn<double>(
            name: "Discount",
            table: "PaymentItems",
            type: "float",
            nullable: false,
            defaultValue: 0.0);
    }
}
