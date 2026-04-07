using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionToUserHasBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Board");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "UserHasBoard",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "UserHasBoard");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Board",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
