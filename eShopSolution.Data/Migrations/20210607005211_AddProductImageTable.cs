using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class AddProductImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 6, 4, 19, 27, 4, 242, DateTimeKind.Local).AddTicks(8794));

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    ImagePath = table.Column<string>(maxLength: 200, nullable: false),
                    Caption = table.Column<string>(maxLength: 200, nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    FileSize = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 6, 4, 19, 27, 4, 242, DateTimeKind.Local).AddTicks(8794),
                oldClrType: typeof(DateTime));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("a2a27184-37cf-462e-977a-0aa2e150ba5e"),
                column: "ConcurrencyStamp",
                value: "5f9de4bf-525f-4836-bf24-9b960cd657b1");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("bc7b1fea-127e-4402-9a0d-41f683d32fce"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "33b00e93-4c26-4bfe-ae56-092765ebf2bd", "AQAAAAEAACcQAAAAEO53kod45EvvZOEuAwNgXLlQiGyK25/0CThhEnwxgkOFtou+iStuZLY4+POZBYdtiw==" });

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
    }
}
