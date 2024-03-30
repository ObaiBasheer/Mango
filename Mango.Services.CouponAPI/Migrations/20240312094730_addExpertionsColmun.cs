using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class addExpertionsColmun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExeprationDate",
                table: "coupons",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 1,
                columns: new[] { "ExeprationDate", "LastUpdate" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 12, 9, 47, 29, 963, DateTimeKind.Utc).AddTicks(762) });

            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 2,
                columns: new[] { "ExeprationDate", "LastUpdate" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 12, 9, 47, 29, 963, DateTimeKind.Utc).AddTicks(809) });

            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 3,
                columns: new[] { "ExeprationDate", "LastUpdate" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 12, 9, 47, 29, 963, DateTimeKind.Utc).AddTicks(822) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExeprationDate",
                table: "coupons");

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

            migrationBuilder.UpdateData(
                table: "coupons",
                keyColumn: "CouponId",
                keyValue: 3,
                column: "LastUpdate",
                value: new DateTime(2024, 2, 16, 10, 45, 58, 505, DateTimeKind.Utc).AddTicks(3082));
        }
    }
}
