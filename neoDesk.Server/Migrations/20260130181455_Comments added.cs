using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neoDesk.Server.Migrations
{
    /// <inheritdoc />
    public partial class Commentsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 28, 19, 14, 55, 334, DateTimeKind.Local).AddTicks(4551));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 19, 14, 55, 334, DateTimeKind.Local).AddTicks(4558));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 14, 55, 164, DateTimeKind.Local).AddTicks(969), "$2a$11$var1yAhH6/qg2lxpPRAAz.WwsYWrm6QhKEAvVrro7Go2cVcTsEiF6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 14, 55, 249, DateTimeKind.Local).AddTicks(4022), "$2a$11$uyT6vFPe1Y3DJ7Snl8/1WOug9xulxzjEAF533XGgyAbCA8Ata6cme" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 14, 55, 334, DateTimeKind.Local).AddTicks(4199), "$2a$11$avpJjQahY4UMuOwxvxzd2.LNbMJXGEMVaicMsnd7fqeioAXrxYlWi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 2, 18, 14, 56, 243, DateTimeKind.Local).AddTicks(6423));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 3, 18, 14, 56, 243, DateTimeKind.Local).AddTicks(6429));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 4, 18, 14, 56, 73, DateTimeKind.Local).AddTicks(7155), "$2a$11$emn9V2E3Pun4VMLAqOgs6utz9.ac1q8Stk9/9.D8MQApAVqqIXxt6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 4, 18, 14, 56, 158, DateTimeKind.Local).AddTicks(7088), "$2a$11$Ru7U782nFpDx4Jcxxq/5J.ji1ZZYFjlFz49VRFg4l2djNEAIhTbvC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 4, 18, 14, 56, 243, DateTimeKind.Local).AddTicks(6109), "$2a$11$4tgJletrFZ46xY.7Nvnf1eDlA0JX.3EUkJNQzU5CfNLL5nPvyfgcq" });
        }
    }
}
