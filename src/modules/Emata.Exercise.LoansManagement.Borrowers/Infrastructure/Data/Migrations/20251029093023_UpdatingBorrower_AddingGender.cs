using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingBorrower_AddingGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                schema: "Borrowers",
                table: "Borrowers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "Borrowers",
                table: "Borrowers");
        }
    }
}
