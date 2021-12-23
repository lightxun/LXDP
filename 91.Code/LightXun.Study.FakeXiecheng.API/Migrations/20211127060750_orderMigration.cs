using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LightXun.Study.FakeXiecheng.API.Migrations
{
    public partial class orderMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "33492df9-c9b5-4bd2-b50e-a0db5d3a67f7", "5e7f510e-cd87-4f8c-9b14-e5cd7a05dae5" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e7f510e-cd87-4f8c-9b14-e5cd7a05dae5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "33492df9-c9b5-4bd2-b50e-a0db5d3a67f7");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "LineItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    CreateDateUTC = table.Column<DateTime>(nullable: false),
                    TransactionMetadata = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ed61ecc6-051c-41d9-a5d8-acfcc835711e", "ce15c8a8-225e-4dfd-a3c2-d8a888e9091a", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c8148135-3eaf-4ca5-9171-f64a191b9392", 0, null, "ef784bee-3762-4dd8-ab51-eb56ce0b1755", "admin@fakexiecheng.com", true, false, null, "ADMIN@FAKEXIECHENG.COM", "ADMIN@FAKEXIECHENG.COM", "AQAAAAEAACcQAAAAEAcj5QWaXVwYsheb3xNt9L82npXIZXL8NumGiNA1wO1+7+IRZpud058tlCZh2GDJ4Q==", "123456789", false, "a10eada6-0866-4c52-8391-d600badecaff", false, "admin@fakexiecheng.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId", "ApplicationUserId" },
                values: new object[] { "c8148135-3eaf-4ca5-9171-f64a191b9392", "ed61ecc6-051c-41d9-a5d8-acfcc835711e", null });

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_OrderId",
                table: "LineItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_Orders_OrderId",
                table: "LineItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_Orders_OrderId",
                table: "LineItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_LineItems_OrderId",
                table: "LineItems");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "c8148135-3eaf-4ca5-9171-f64a191b9392", "ed61ecc6-051c-41d9-a5d8-acfcc835711e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed61ecc6-051c-41d9-a5d8-acfcc835711e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c8148135-3eaf-4ca5-9171-f64a191b9392");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "LineItems");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5e7f510e-cd87-4f8c-9b14-e5cd7a05dae5", "5020c4e1-c82b-4502-a365-86fd59624d2b", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "33492df9-c9b5-4bd2-b50e-a0db5d3a67f7", 0, null, "fc53502d-4ec4-4eb6-9160-1a1f4a579e76", "admin@fakexiecheng.com", true, false, null, "ADMIN@FAKEXIECHENG.COM", "ADMIN@FAKEXIECHENG.COM", "AQAAAAEAACcQAAAAEC96WGOGQy3MABIcFHClQCu/N/clydqx03XT9v1z2kls/YzbNaVwZcpw86KOECUalg==", "123456789", false, "a0673a88-9452-4676-8f73-848657df69a8", false, "admin@fakexiecheng.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId", "ApplicationUserId" },
                values: new object[] { "33492df9-c9b5-4bd2-b50e-a0db5d3a67f7", "5e7f510e-cd87-4f8c-9b14-e5cd7a05dae5", null });
        }
    }
}
