using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LightXun.Study.FakeXiecheng.API.Migrations
{
    public partial class ShoppingCartMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "641854d9-83ff-49c5-b15f-938d3c8d4d98", "e4c3d3ad-4922-4184-a02e-717ef34f735b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e4c3d3ad-4922-4184-a02e-717ef34f735b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "641854d9-83ff-49c5-b15f-938d3c8d4d98");

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LineItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TouristRouteId = table.Column<Guid>(nullable: false),
                    ShoppingCartId = table.Column<Guid>(nullable: true),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPresent = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineItems_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LineItems_TouristRoutes_TouristRouteId",
                        column: x => x.TouristRouteId,
                        principalTable: "TouristRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_ShoppingCartId",
                table: "LineItems",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_TouristRouteId",
                table: "LineItems",
                column: "TouristRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_UserId",
                table: "ShoppingCarts",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineItems");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e4c3d3ad-4922-4184-a02e-717ef34f735b", "baa75950-6d91-4cb0-ac7a-8466c4d90e21", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "641854d9-83ff-49c5-b15f-938d3c8d4d98", 0, null, "e1991057-9ba7-4002-8d38-a31793d3fd67", "admin@fakexiecheng.com", true, false, null, "ADMIN@FAKEXIECHENG.COM", "ADMIN@FAKEXIECHENG.COM", "AQAAAAEAACcQAAAAEMiap7p4faqLqZnqWjbwPp7h1rFkaY8jU525kDtbE3NVz9zZ6mFJyzSbNsTnpGjSLg==", "123456789", false, "17839b31-c479-4fcd-ac9b-412ee8843b68", false, "admin@fakexiecheng.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId", "ApplicationUserId" },
                values: new object[] { "641854d9-83ff-49c5-b15f-938d3c8d4d98", "e4c3d3ad-4922-4184-a02e-717ef34f735b", null });
        }
    }
}
