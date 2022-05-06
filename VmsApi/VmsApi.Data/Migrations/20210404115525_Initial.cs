using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VmsApi.Data.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PigGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    GroupNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PigGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<float>(nullable: false),
                    PigGroupId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodPurchases_PigGroups_PigGroupId",
                        column: x => x.PigGroupId,
                        principalTable: "PigGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MeasurePoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Weight = table.Column<float>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    PigGroupId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurePoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurePoints_PigGroups_PigGroupId",
                        column: x => x.PigGroupId,
                        principalTable: "PigGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TaskTitle = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    Completed = table.Column<bool>(nullable: false),
                    RepeatingIntervalDays = table.Column<int>(nullable: false),
                    CreatorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItems_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignedTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TaskItemId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignedTask_TaskItems_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "TaskItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignedTask_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0eb56564-4c92-4259-ab6f-6a9912c5c0c3", "114d5955-1253-4d45-8254-650da3fae9d7", "Administrator", "ADMINISTRATOR" },
                    { "36c604a2-1f4e-4552-8741-74140540679b", "b2c80e0a-6047-4163-baeb-90267ff0f89d", "Administratief_medewerker", "ACCOUNTANT" },
                    { "8cac7619-270a-4591-add3-4f8b983ed286", "c2581911-7e10-482e-8c80-dff1b2e53a41", "Agendabeheerder", "AGENDA" },
                    { "232bf85e-2c94-4121-96ac-51688c8a1189", "ac3acad2-db81-46d3-bd9f-a0481052ba3f", "Chief_Executive_Officer", "CEO" },
                    { "a5d32981 - f064 - 4277 - b061 - f4dc6eba9ef5", "0556721e-ad42-4b25-af6b-19355fba53b0", "Manager", "MANAGER" },
                    { "afc3df08-7aa2-45a0-b2ad-e35e2c344c6f", "b81d6342-d493-42ce-a16e-0433b5bf5e1b", "Voedingsbeheerder", "NUTRITION" },
                    { "b645b107-532b-4c9a-a1f0-d807dbac63b0", "5651096c-c18f-425f-a3d2-1318711bc69b", "Weger", "WEGER" },
                    { "b888e790-e6f8-49ce-b5b5-bab5ee97593a", "7d597a2a-9559-4382-bf0f-d3a445de8381", "Werknemer", "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "626ea8c4-2ea4-45c6-8c08-c522eca14b00", "c54eb66f-4a09-4320-b9eb-0a9c6ae1d39a" },
                    { "626ea8c4-2ea4-45c6-8c08-c522eca14b00", "b888e790-e6f8-49ce-b5b5-bab5ee97593a" },
                    { "626ea8c4-2ea4-45c6-8c08-c522eca14b00", "b645b107-532b-4c9a-a1f0-d807dbac63b0" },
                    { "626ea8c4-2ea4-45c6-8c08-c522eca14b00", "afc3df08-7aa2-45a0-b2ad-e35e2c344c6f" },
                    { "626ea8c4-2ea4-45c6-8c08-c522eca14b00", "36c604a2-1f4e-4552-8741-74140540679b" },
                    { "626ea8c4-2ea4-45c6-8c08-c522eca14b00", "8cac7619-270a-4591-add3-4f8b983ed286" },
                    { "626ea8c4-2ea4-45c6-8c08-c522eca14b00", "0eb56564-4c92-4259-ab6f-6a9912c5c0c3" },
                    { "626ea8c4-2ea4-45c6-8c08-c522eca14b00", "a5d32981-f064-4277-b061-f4dc6eba9ef5" }
                });

            migrationBuilder.InsertData(
                table: "TaskItems",
                columns: new[] { "Id", "Completed", "CreatorId", "Description", "DueDate", "RepeatingIntervalDays", "StartDate", "TaskTitle" },
                values: new object[,]
                {
                    { new Guid("626ea8c4-2ea4-45c6-8c08-c522eaaaaaaa"), true, null, "Anders kunnen ze niet feesten.", new DateTime(2021, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2021, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Varkens vaccineren tegen Corona" },
                    { new Guid("626ea8c4-2ea4-45c6-8c08-c522eca14b0a"), false, null, "Het voer ligt in de schuur.", new DateTime(2021, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, new DateTime(2021, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Varkens voederen" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "626ea8c4-2ea4-45c6-8c08-c522eca14b00", 0, "687a9571-1098-4e18-bedc-f7075207a573", "xxxx@example.com", true, "John", "Doe", false, null, "XXXX@EXAMPLE.COM", "OWNER", "AQAAAAEAACcQAAAAEGpaVsQ7M5pomvpiM9HvM4/i8tDIyNp2hU9LY8dCggz4FdHwnqhRFwyW5/zNJnN4aw==", "+111111111111", true, "96bd606a-2d23-4cc7-a929-e380d181d0e6", false, "Owner" });

            migrationBuilder.InsertData(
                table: "AssignedTask",
                columns: new[] { "Id", "TaskItemId", "UserId" },
                values: new object[] { new Guid("626ea8c4-3333-45c6-8c08-c522eca14b0a"), new Guid("626ea8c4-2ea4-45c6-8c08-c522eca14b0a"), "626ea8c4-2ea4-45c6-8c08-c522eca14b00" });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTask_TaskItemId",
                table: "AssignedTask",
                column: "TaskItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTask_UserId",
                table: "AssignedTask",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodPurchases_PigGroupId",
                table: "FoodPurchases",
                column: "PigGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurePoints_PigGroupId",
                table: "MeasurePoints",
                column: "PigGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PigGroups_GroupNumber",
                table: "PigGroups",
                column: "GroupNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_CreatorId",
                table: "TaskItems",
                column: "CreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedTask");

            migrationBuilder.DropTable(
                name: "FoodPurchases");

            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.DropTable(
                name: "MeasurePoints");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "PigGroups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
