using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorUserDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "UserDevice");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "UserDevice");

            migrationBuilder.DropColumn(
                name: "IsLogout",
                table: "UserDevice");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "UserDevice");

            migrationBuilder.DropColumn(
                name: "OS",
                table: "UserDevice");

            migrationBuilder.DropColumn(
                name: "UniqulyIdentifier",
                table: "UserDevice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "UserDevice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "UserDevice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsLogout",
                table: "UserDevice",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "UserDevice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OS",
                table: "UserDevice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniqulyIdentifier",
                table: "UserDevice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
