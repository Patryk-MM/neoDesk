using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neoDesk.Server.Migrations
{
    /// <inheritdoc />
    public partial class LastUpdatedAtadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Tickets",
                newName: "LastUpdatedAt");

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LastUpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 25, 21, 950, DateTimeKind.Local).AddTicks(6592), new DateTime(2026, 1, 30, 19, 25, 21, 950, DateTimeKind.Local).AddTicks(6595) });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "LastUpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 31, 19, 25, 21, 950, DateTimeKind.Local).AddTicks(6604), new DateTime(2026, 1, 31, 19, 25, 21, 950, DateTimeKind.Local).AddTicks(6605) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 1, 19, 25, 21, 779, DateTimeKind.Local).AddTicks(9017), "$2a$11$.kuw3yiGvBZScszOmwrvGOaPedf32.TcIrYQXkEEjqD7vfQmAEHOS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 1, 19, 25, 21, 865, DateTimeKind.Local).AddTicks(4845), "$2a$11$PDAj/1A1ZSCR9bXAlz3cyu.IHmQcjueG2p1vCwLrNzj8x8OBTdPTG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 2, 1, 19, 25, 21, 950, DateTimeKind.Local).AddTicks(6214), "$2a$11$j5FxqvKDvlR1OcdqqMu3NeIze51HlA1NktRtvLhiQVCl88/c8xfTu" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdatedAt",
                table: "Tickets",
                newName: "UpdatedAt");

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 28, 19, 54, 34, 951, DateTimeKind.Local).AddTicks(1179), null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 29, 19, 54, 34, 951, DateTimeKind.Local).AddTicks(1187), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 54, 34, 780, DateTimeKind.Local).AddTicks(3990), "$2a$11$SqQnYsxgopJSF7OlAT7Tvuqfx651Ir2WX2HTobqQqHbQJAOqhvORG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 54, 34, 865, DateTimeKind.Local).AddTicks(8277), "$2a$11$VoK5y6n0EoyMKQAj7wbTV.biBERxLpzQLGAYi5y8C1TKHuPaueacW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 54, 34, 951, DateTimeKind.Local).AddTicks(875), "$2a$11$lJymyZfvQM1m1nsSnuAnr.hwSypDIU2HQC5Dhnys8/vaGPfnNhOIa" });
        }
    }
}
