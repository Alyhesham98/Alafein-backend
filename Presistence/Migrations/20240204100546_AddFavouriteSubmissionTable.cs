using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFavouriteSubmissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavouriteSubmission",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubmissionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteSubmission", x => new { x.SubmissionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FavouriteSubmission_Submission_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavouriteSubmission_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteSubmission_UserId",
                table: "FavouriteSubmission",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouriteSubmission");
        }
    }
}
