using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    NotificationId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationHistory_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationHistory_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "security",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationHistory_NotificationId",
                table: "NotificationHistory",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationHistory_UserId",
                table: "NotificationHistory",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationHistory");
        }
    }
}
