using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neoDesk.Server.Migrations
{
    /// <inheritdoc />
    public partial class commentrefined : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TicketId",
                table: "Comments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tickets_TicketId",
                table: "Comments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tickets_TicketId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_TicketId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Comments");

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
    }
}
