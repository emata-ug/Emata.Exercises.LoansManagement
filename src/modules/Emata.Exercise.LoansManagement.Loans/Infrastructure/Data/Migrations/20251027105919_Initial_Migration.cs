using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emata.Exercise.LoansManagement.Loans.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Loans");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "Loans",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Loans",
                schema: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    LoanAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LoanApplicationId = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    Duration = table.Column<string>(type: "jsonb", nullable: true),
                    InterestRate = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans",
                schema: "Loans");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "Loans");
        }
    }
}
