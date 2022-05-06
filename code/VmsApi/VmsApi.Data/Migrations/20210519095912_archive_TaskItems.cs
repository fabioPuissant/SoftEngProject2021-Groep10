using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VmsApi.Data.Migrations
{
    public partial class archive_TaskItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Users_CreatorId",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_CreatorId",
                table: "TaskItems");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "232bf85e-2c94-4121-96ac-51688c8a1189");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "TaskItems");

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "TaskItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "0eb56564-4c92-4259-ab6f-6a9912c5c0c3",
                column: "ConcurrencyStamp",
                value: "553cea7b-a181-4cb4-a24e-92199d12b76a");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "36c604a2-1f4e-4552-8741-74140540679b",
                column: "ConcurrencyStamp",
                value: "86b6171f-e210-46ba-8a39-0e766e853f4e");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "8cac7619-270a-4591-add3-4f8b983ed286",
                column: "ConcurrencyStamp",
                value: "46f8a92a-037b-442b-8be7-ff80c8b9ee52");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a5d32981 - f064 - 4277 - b061 - f4dc6eba9ef5",
                column: "ConcurrencyStamp",
                value: "6bbf2437-67c1-4128-b398-983859e485a5");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "afc3df08-7aa2-45a0-b2ad-e35e2c344c6f",
                column: "ConcurrencyStamp",
                value: "28f3b67d-6d19-4c85-a86d-49548a0d44da");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b645b107-532b-4c9a-a1f0-d807dbac63b0",
                column: "ConcurrencyStamp",
                value: "f929273e-3b18-4be3-a970-6e911d4f72f2");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b888e790-e6f8-49ce-b5b5-bab5ee97593a",
                column: "ConcurrencyStamp",
                value: "e3a74c5d-9db2-47d4-ab2a-2bed56a37fa1");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "97a29775-d22a-40d9-b65e-fc6cb10b35c8", "1b17590d-c9f6-4285-9716-ddd1b7d7cc71", "Chief_Executive_Officer", "CEO" });

            migrationBuilder.UpdateData(
                table: "TaskItems",
                keyColumn: "Id",
                keyValue: new Guid("626ea8c4-2ea4-45c6-8c08-c522eaaaaaaa"),
                column: "StartDate",
                value: new DateTime(2021, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TaskItems",
                keyColumn: "Id",
                keyValue: new Guid("626ea8c4-2ea4-45c6-8c08-c522eca14b0a"),
                column: "StartDate",
                value: new DateTime(2021, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "97a29775-d22a-40d9-b65e-fc6cb10b35c8");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "TaskItems");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "TaskItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "0eb56564-4c92-4259-ab6f-6a9912c5c0c3",
                column: "ConcurrencyStamp",
                value: "114d5955-1253-4d45-8254-650da3fae9d7");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "36c604a2-1f4e-4552-8741-74140540679b",
                column: "ConcurrencyStamp",
                value: "b2c80e0a-6047-4163-baeb-90267ff0f89d");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "8cac7619-270a-4591-add3-4f8b983ed286",
                column: "ConcurrencyStamp",
                value: "c2581911-7e10-482e-8c80-dff1b2e53a41");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "a5d32981 - f064 - 4277 - b061 - f4dc6eba9ef5",
                column: "ConcurrencyStamp",
                value: "0556721e-ad42-4b25-af6b-19355fba53b0");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "afc3df08-7aa2-45a0-b2ad-e35e2c344c6f",
                column: "ConcurrencyStamp",
                value: "b81d6342-d493-42ce-a16e-0433b5bf5e1b");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b645b107-532b-4c9a-a1f0-d807dbac63b0",
                column: "ConcurrencyStamp",
                value: "5651096c-c18f-425f-a3d2-1318711bc69b");

            migrationBuilder.UpdateData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "b888e790-e6f8-49ce-b5b5-bab5ee97593a",
                column: "ConcurrencyStamp",
                value: "7d597a2a-9559-4382-bf0f-d3a445de8381");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "232bf85e-2c94-4121-96ac-51688c8a1189", "ac3acad2-db81-46d3-bd9f-a0481052ba3f", "Chief_Executive_Officer", "CEO" });

            migrationBuilder.UpdateData(
                table: "TaskItems",
                keyColumn: "Id",
                keyValue: new Guid("626ea8c4-2ea4-45c6-8c08-c522eaaaaaaa"),
                column: "StartDate",
                value: new DateTime(2021, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TaskItems",
                keyColumn: "Id",
                keyValue: new Guid("626ea8c4-2ea4-45c6-8c08-c522eca14b0a"),
                column: "StartDate",
                value: new DateTime(2021, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_CreatorId",
                table: "TaskItems",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Users_CreatorId",
                table: "TaskItems",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
