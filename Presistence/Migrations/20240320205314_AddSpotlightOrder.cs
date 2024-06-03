using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSpotlightOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpotlightOrder",
                table: "SubmissionDate",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpotlightOrder",
                table: "SubmissionDate");
        }
    }
}
