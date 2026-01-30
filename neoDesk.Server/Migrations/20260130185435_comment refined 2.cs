using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neoDesk.Server.Migrations
{
    /// <inheritdoc />
    public partial class commentrefined2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 28, 19, 54, 34, 951, DateTimeKind.Local).AddTicks(1179));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 19, 54, 34, 951, DateTimeKind.Local).AddTicks(1187));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 28, 19, 20, 8, 924, DateTimeKind.Local).AddTicks(9493));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 19, 20, 8, 924, DateTimeKind.Local).AddTicks(9500));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 20, 8, 753, DateTimeKind.Local).AddTicks(9711), "$2a$11$zVfbWlf9qVKkjdnAXYp4x.9nKZdkBOtpaG8ivg69eGyHtQ5vLOYma" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 20, 8, 838, DateTimeKind.Local).AddTicks(9725), "$2a$11$SUz6s/OwViFLzVYm3nS6d.i78vIsP/Xb0n1FAjLu3T60NNbbBQe7." });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 20, 8, 924, DateTimeKind.Local).AddTicks(9057), "$2a$11$jIGBVs8FF.Q6xg3sX9/R7uEb47GPmG2s4MvuPpBdHfnYR32cGW3UO" });
        }
    }
}
