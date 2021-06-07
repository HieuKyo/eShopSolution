using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class ChangeFileLengthType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "ProductImages",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a2a27184-37cf-462e-977a-0aa2e150ba5e"),
                column: "ConcurrencyStamp",
                value: "265c36e7-d701-43a6-864c-b4469c03676c");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bc7b1fea-127e-4402-9a0d-41f683d32fce"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "295e5476-7b93-427f-a76e-4fb6d2719b51", "AQAAAAEAACcQAAAAEGv9d/yqg9gQ5Fi44436XnPqk+26u9EaNkBFyQb5mupJi6GkwgdL5fzo7fh/mQHHFA==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 7, 9, 10, 14, 973, DateTimeKind.Local).AddTicks(8199));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "ProductImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a2a27184-37cf-462e-977a-0aa2e150ba5e"),
                column: "ConcurrencyStamp",
                value: "3d0a7c87-7638-4fb4-a4b9-4e8f98ac1a89");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bc7b1fea-127e-4402-9a0d-41f683d32fce"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2028c49c-00a7-43d0-85a6-f7ea911b608e", "AQAAAAEAACcQAAAAEE/J8lMM9MRhSWtP0rRWYPBfsHdESSpbQk7S7eO+Acy/z96TwqrbLtiXkwAkFjv0+g==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 7, 7, 52, 11, 51, DateTimeKind.Local).AddTicks(1163));
        }
    }
}
