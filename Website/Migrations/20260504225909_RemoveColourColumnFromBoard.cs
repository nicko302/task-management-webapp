using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_Application.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColourColumnFromBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Colour",
                table: "Board");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedAt",
                table: "Task",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "Task",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Task",
                keyColumn: "UpdatedAt",
                keyValue: null,
                column: "UpdatedAt",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedAt",
                table: "Task",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Task",
                keyColumn: "CreatedAt",
                keyValue: null,
                column: "CreatedAt",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "Task",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Colour",
                table: "Board",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
