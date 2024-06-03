using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorFavouriteSubmissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FavouriteSubmission",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<long>(
                name: "SubmissionId",
                table: "FavouriteSubmission",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FavouriteSubmission",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<long>(
                name: "SubmissionId",
                table: "FavouriteSubmission",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Relational:ColumnOrder", 2);
        }
    }
}
