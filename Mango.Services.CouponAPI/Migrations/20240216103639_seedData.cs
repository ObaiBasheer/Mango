using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 10, 36, 39, 158, DateTimeKind.Utc).AddTicks(1114));

            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 10, 36, 39, 158, DateTimeKind.Utc).AddTicks(1152));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 14, 13, 26, 22, 411, DateTimeKind.Utc).AddTicks(2615));

            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 14, 13, 26, 22, 411, DateTimeKind.Utc).AddTicks(2655));
        }
    }
}
