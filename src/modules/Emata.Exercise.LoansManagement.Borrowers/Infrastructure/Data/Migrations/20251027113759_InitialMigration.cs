using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Borrowers");

            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "Borrowers",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Partners",
                schema: "Borrowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Borrowers",
                schema: "Borrowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GivenName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false, computedColumnSql: "    \"GivenName\" || ' ' || \"Surname\"", stored: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    IdentificationNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    PartnerId = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Borrowers_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalSchema: "Borrowers",
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Borrowers_PartnerId",
                schema: "Borrowers",
                table: "Borrowers",
                column: "PartnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Borrowers",
                schema: "Borrowers");

            migrationBuilder.DropTable(
                name: "Partners",
                schema: "Borrowers");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence",
                schema: "Borrowers");
        }
    }
}
