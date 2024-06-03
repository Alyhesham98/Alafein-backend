using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionStatusAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Submission",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Submission",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Submission_UserId",
                table: "Submission",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submission_User_UserId",
                table: "Submission",
                column: "UserId",
                principalSchema: "security",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submission_User_UserId",
                table: "Submission");

            migrationBuilder.DropIndex(
                name: "IX_Submission_UserId",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Submission");
        }
    }
}
