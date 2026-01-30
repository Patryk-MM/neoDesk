using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neoDesk.Server.Migrations
{
    /// <inheritdoc />
    public partial class TicketIdaddedtoComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 28, 19, 15, 52, 412, DateTimeKind.Local).AddTicks(2403));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 29, 19, 15, 52, 412, DateTimeKind.Local).AddTicks(2409));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 15, 52, 241, DateTimeKind.Local).AddTicks(6325), "$2a$11$p8HLJoYZpplkoFyKbTywEOqCDG3SL2JZeHXgi6g9mYZgrPCcQ8U2S" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 15, 52, 326, DateTimeKind.Local).AddTicks(9040), "$2a$11$JPsSXGDbGydIlqoMf5YJBuOA8vQv3sXOFEPDRnq/Ndp7X36SXFoFa" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 1, 30, 19, 15, 52, 412, DateTimeKind.Local).AddTicks(2090), "$2a$11$z6dPgpX0JRvuO3TBeokw0uz3zPEs8VprJkDrn1BcNgXHIyiX8dI/S" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
