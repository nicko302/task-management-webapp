using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_Application.Migrations
{
    /// <inheritdoc />
    public partial class RenameIdentityTablesAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Roles_RoleId",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Users_UserId",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_Users_UserId",
                table: "UserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_Users_UserId",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogins",
                table: "UserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleClaims",
                table: "RoleClaims");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                newName: "iUserTokens");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "iUserRoles");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                newName: "iUserLogins");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "iUserClaims");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "iRoles");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                newName: "iRoleClaims");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                table: "iUserRoles",
                newName: "IX_iUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLogins_UserId",
                table: "iUserLogins",
                newName: "IX_iUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserClaims_UserId",
                table: "iUserClaims",
                newName: "IX_iUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleClaims_RoleId",
                table: "iRoleClaims",
                newName: "IX_iRoleClaims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_iUserTokens",
                table: "iUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_iUserRoles",
                table: "iUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_iUserLogins",
                table: "iUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_iUserClaims",
                table: "iUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_iRoles",
                table: "iRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_iRoleClaims",
                table: "iRoleClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_iRoleClaims_iRoles_RoleId",
                table: "iRoleClaims",
                column: "RoleId",
                principalTable: "iRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_iUserClaims_Users_UserId",
                table: "iUserClaims",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_iUserLogins_Users_UserId",
                table: "iUserLogins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_iUserRoles_Users_UserId",
                table: "iUserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_iUserRoles_iRoles_RoleId",
                table: "iUserRoles",
                column: "RoleId",
                principalTable: "iRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_iUserTokens_Users_UserId",
                table: "iUserTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_iRoleClaims_iRoles_RoleId",
                table: "iRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_iUserClaims_Users_UserId",
                table: "iUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_iUserLogins_Users_UserId",
                table: "iUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_iUserRoles_Users_UserId",
                table: "iUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_iUserRoles_iRoles_RoleId",
                table: "iUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_iUserTokens_Users_UserId",
                table: "iUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_iUserTokens",
                table: "iUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_iUserRoles",
                table: "iUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_iUserLogins",
                table: "iUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_iUserClaims",
                table: "iUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_iRoles",
                table: "iRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_iRoleClaims",
                table: "iRoleClaims");

            migrationBuilder.RenameTable(
                name: "iUserTokens",
                newName: "UserTokens");

            migrationBuilder.RenameTable(
                name: "iUserRoles",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "iUserLogins",
                newName: "UserLogins");

            migrationBuilder.RenameTable(
                name: "iUserClaims",
                newName: "UserClaims");

            migrationBuilder.RenameTable(
                name: "iRoles",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "iRoleClaims",
                newName: "RoleClaims");

            migrationBuilder.RenameIndex(
                name: "IX_iUserRoles_RoleId",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_iUserLogins_UserId",
                table: "UserLogins",
                newName: "IX_UserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_iUserClaims_UserId",
                table: "UserClaims",
                newName: "IX_UserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_iRoleClaims_RoleId",
                table: "RoleClaims",
                newName: "IX_RoleClaims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogins",
                table: "UserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleClaims",
                table: "RoleClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Roles_RoleId",
                table: "RoleClaims",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Users_UserId",
                table: "UserClaims",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_Users_UserId",
                table: "UserLogins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_Users_UserId",
                table: "UserTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
