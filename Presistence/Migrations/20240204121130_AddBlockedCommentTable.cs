using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockedCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlockedFromComment",
                schema: "security",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "BlockedComment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmissionCommentId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlockedComment_SubmissionComment_SubmissionCommentId",
                        column: x => x.SubmissionCommentId,
                        principalTable: "SubmissionComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockedComment_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockedComment_SubmissionCommentId",
                table: "BlockedComment",
                column: "SubmissionCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockedComment_UserId",
                table: "BlockedComment",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockedComment");

            migrationBuilder.DropColumn(
                name: "IsBlockedFromComment",
                schema: "security",
                table: "User");
        }
    }
}
