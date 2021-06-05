using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class SeedIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new Guid("8d04dce2-969a-435d-bba4-df3f325983dc") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(2021, 6, 4, 19, 27, 4, 242, DateTimeKind.Local).AddTicks(8794),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 6, 4, 19, 19, 32, 361, DateTimeKind.Local).AddTicks(3528));

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("a2a27184-37cf-462e-977a-0aa2e150ba5e"), "5f9de4bf-525f-4836-bf24-9b960cd657b1", "Administrator role", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("bc7b1fea-127e-4402-9a0d-41f683d32fce"), new Guid("a2a27184-37cf-462e-977a-0aa2e150ba5e") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("bc7b1fea-127e-4402-9a0d-41f683d32fce"), 0, "33b00e93-4c26-4bfe-ae56-092765ebf2bd", new DateTime(1999, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "hieutt291199@gmail.com", true, "Hieu", "Truong", false, null, "hieutt291199@gmail.com", "admin", "AQAAAAEAACcQAAAAEO53kod45EvvZOEuAwNgXLlQiGyK25/0CThhEnwxgkOFtou+iStuZLY4+POZBYdtiw==", null, false, "", false, "admin" });

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
                value: new DateTime(2021, 6, 4, 19, 27, 4, 262, DateTimeKind.Local).AddTicks(3188));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a2a27184-37cf-462e-977a-0aa2e150ba5e"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("bc7b1fea-127e-4402-9a0d-41f683d32fce"), new Guid("a2a27184-37cf-462e-977a-0aa2e150ba5e") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bc7b1fea-127e-4402-9a0d-41f683d32fce"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 6, 4, 19, 19, 32, 361, DateTimeKind.Local).AddTicks(3528),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 6, 4, 19, 27, 4, 242, DateTimeKind.Local).AddTicks(8794));

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), "df4bb59e-63b6-48f3-812b-4f0c98d9e026", "Administrator role", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new Guid("8d04dce2-969a-435d-bba4-df3f325983dc") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), 0, "5e0daff2-9815-472d-87ac-5834ae7d9160", new DateTime(1999, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "hieutt291199@gmail.com", true, "Hieu", "Truong", false, null, "hieutt291199@gmail.com", "admin", "AQAAAAEAACcQAAAAEEYHUJKV2YsApi3m4IOZYm6MPnDMZSlwuAjPQKwpOd6pD4XRAxpkQHtLcHK/+kFcYA==", null, false, "", false, "admin" });

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
                value: new DateTime(2021, 6, 4, 19, 19, 32, 405, DateTimeKind.Local).AddTicks(4995));
        }
    }
}
