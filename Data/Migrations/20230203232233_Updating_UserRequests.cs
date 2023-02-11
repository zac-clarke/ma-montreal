using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaMontreal.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingUserRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_AspNetRoles_RoleRequestedId",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "UserRequests");

            migrationBuilder.AlterColumn<string>(
                name: "RoleRequestedId",
                table: "UserRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequests_AspNetRoles_RoleRequestedId",
                table: "UserRequests",
                column: "RoleRequestedId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_AspNetRoles_RoleRequestedId",
                table: "UserRequests");

            migrationBuilder.AlterColumn<string>(
                name: "RoleRequestedId",
                table: "UserRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "UserRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequests_AspNetRoles_RoleRequestedId",
                table: "UserRequests",
                column: "RoleRequestedId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
