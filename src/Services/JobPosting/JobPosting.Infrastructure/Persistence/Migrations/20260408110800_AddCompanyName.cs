using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPosting.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Job_Postings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Job_Postings");
        }
    }
}
