using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emata.Exercise.LoansManagement.Loans.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Loans_UpdatedAndAddedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoanApplicationId",
                schema: "Loans",
                table: "Loans",
                newName: "Reference");

            migrationBuilder.AddColumn<int>(
                name: "BorrowerId",
                schema: "Loans",
                table: "Loans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                schema: "Loans",
                table: "Loans",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowerId",
                schema: "Loans",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "Reason",
                schema: "Loans",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "Reference",
                schema: "Loans",
                table: "Loans",
                newName: "LoanApplicationId");
        }
    }
}
