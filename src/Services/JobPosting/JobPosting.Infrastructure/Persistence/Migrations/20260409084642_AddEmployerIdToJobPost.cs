using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPosting.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployerIdToJobPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Job_Postings",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "Job_Postings",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "EmployerEmail",
                table: "Job_Postings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployerId",
                table: "Job_Postings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployerEmail",
                table: "Job_Postings");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "Job_Postings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostDate",
                table: "Job_Postings",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "Job_Postings",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
