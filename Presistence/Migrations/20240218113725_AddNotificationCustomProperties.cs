using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationCustomProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomColor",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomImageUrl",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomTextColor",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomColor",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "CustomImageUrl",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "CustomTextColor",
                table: "Notification");
        }
    }
}
