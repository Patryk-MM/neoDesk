using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace neoDesk.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    AssignedToUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "LastLoginAt", "Name", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 4, 18, 14, 56, 73, DateTimeKind.Local).AddTicks(7155), "admin@neodesk.com", true, null, "Administrator", "$2a$11$emn9V2E3Pun4VMLAqOgs6utz9.ac1q8Stk9/9.D8MQApAVqqIXxt6", 2 },
                    { 2, new DateTime(2025, 11, 4, 18, 14, 56, 158, DateTimeKind.Local).AddTicks(7088), "jan.kowalski@company.com", true, null, "Jan Kowalski", "$2a$11$Ru7U782nFpDx4Jcxxq/5J.ji1ZZYFjlFz49VRFg4l2djNEAIhTbvC", 0 },
                    { 3, new DateTime(2025, 11, 4, 18, 14, 56, 243, DateTimeKind.Local).AddTicks(6109), "anna.nowak@company.com", true, null, "Anna Nowak", "$2a$11$4tgJletrFZ46xY.7Nvnf1eDlA0JX.3EUkJNQzU5CfNLL5nPvyfgcq", 1 }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssignedToUserId", "Category", "CreatedAt", "CreatedByUserId", "Description", "Status", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, null, 1, new DateTime(2025, 11, 2, 18, 14, 56, 243, DateTimeKind.Local).AddTicks(6423), 1, "Drukarka nie drukuje dokumentów", 0, "Problem z drukarką", null },
                    { 2, 3, 0, new DateTime(2025, 11, 3, 18, 14, 56, 243, DateTimeKind.Local).AddTicks(6429), 2, "Aplikacja CRM wyświetla błąd przy logowaniu", 1, "Błąd w aplikacji", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AssignedToUserId",
                table: "Tickets",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreatedByUserId",
                table: "Tickets",
                column: "CreatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
