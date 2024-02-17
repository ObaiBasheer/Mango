using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedData1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 10, 45, 58, 505, DateTimeKind.Utc).AddTicks(3042));

            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 10, 45, 58, 505, DateTimeKind.Utc).AddTicks(3072));

            migrationBuilder.InsertData(
                table: "coupons",
                columns: new[] { "CouponId", "CouponCode", "DiscountAmount", "LastUpdate", "MinAmount" },
                values: new object[] { 3, "30OFF", 10.0, new DateTime(2024, 2, 16, 10, 45, 58, 505, DateTimeKind.Utc).AddTicks(3082), 50 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 10, 43, 13, 340, DateTimeKind.Utc).AddTicks(777));

            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 2,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 10, 43, 13, 340, DateTimeKind.Utc).AddTicks(818));
        }
    }
}
