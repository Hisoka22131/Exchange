using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Exchange.Database.Migrations
{
    /// <inheritdoc />
    public partial class update_table_transactions_add_column_amountInUsdt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: new Guid("6b6c857a-7dc3-4e6b-8ab7-5f081631df54"));

            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: new Guid("89b9c1c2-bd3b-4a90-9ea8-ac3f2d24d4cf"));

            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: new Guid("ff0aeb7d-4a4f-498f-9593-817d4086c09b"));

            migrationBuilder.AddColumn<decimal>(
                name: "AmountToInUsdt",
                table: "Transactions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Commissions",
                columns: new[] { "Id", "AmountFrom", "AmountTo", "CreatedAt", "Currency", "FixedFee", "PercentFee", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1357dce9-0c7b-4bcd-befc-1dbb0f11c9cb"), 0m, 400m, new DateTimeOffset(new DateTime(2025, 2, 21, 17, 11, 5, 857, DateTimeKind.Unspecified).AddTicks(2973), new TimeSpan(0, 0, 0, 0, 0)), "USDT", 20m, 0m, new DateTimeOffset(new DateTime(2025, 2, 21, 17, 11, 5, 857, DateTimeKind.Unspecified).AddTicks(3036), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("2f537bf1-d79c-487e-9247-b421ac051e00"), 400m, 800m, new DateTimeOffset(new DateTime(2025, 2, 21, 17, 11, 5, 857, DateTimeKind.Unspecified).AddTicks(3043), new TimeSpan(0, 0, 0, 0, 0)), "USDT", null, 0.15m, new DateTimeOffset(new DateTime(2025, 2, 21, 17, 11, 5, 857, DateTimeKind.Unspecified).AddTicks(3045), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("aeee4387-dca7-47a0-bffa-f6c6fc8f4bd8"), 800m, null, new DateTimeOffset(new DateTime(2025, 2, 21, 17, 11, 5, 857, DateTimeKind.Unspecified).AddTicks(3059), new TimeSpan(0, 0, 0, 0, 0)), "USDT", null, 0.05m, new DateTimeOffset(new DateTime(2025, 2, 21, 17, 11, 5, 857, DateTimeKind.Unspecified).AddTicks(3061), new TimeSpan(0, 0, 0, 0, 0)) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: new Guid("1357dce9-0c7b-4bcd-befc-1dbb0f11c9cb"));

            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: new Guid("2f537bf1-d79c-487e-9247-b421ac051e00"));

            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: new Guid("aeee4387-dca7-47a0-bffa-f6c6fc8f4bd8"));

            migrationBuilder.DropColumn(
                name: "AmountToInUsdt",
                table: "Transactions");

            migrationBuilder.InsertData(
                table: "Commissions",
                columns: new[] { "Id", "AmountFrom", "AmountTo", "CreatedAt", "Currency", "FixedFee", "PercentFee", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("6b6c857a-7dc3-4e6b-8ab7-5f081631df54"), 800m, null, new DateTimeOffset(new DateTime(2025, 2, 17, 17, 56, 2, 709, DateTimeKind.Unspecified).AddTicks(5179), new TimeSpan(0, 0, 0, 0, 0)), "USDT", null, 0.05m, new DateTimeOffset(new DateTime(2025, 2, 17, 17, 56, 2, 709, DateTimeKind.Unspecified).AddTicks(5180), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("89b9c1c2-bd3b-4a90-9ea8-ac3f2d24d4cf"), 0m, 400m, new DateTimeOffset(new DateTime(2025, 2, 17, 17, 56, 2, 709, DateTimeKind.Unspecified).AddTicks(5104), new TimeSpan(0, 0, 0, 0, 0)), "USDT", 20m, 0m, new DateTimeOffset(new DateTime(2025, 2, 17, 17, 56, 2, 709, DateTimeKind.Unspecified).AddTicks(5156), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("ff0aeb7d-4a4f-498f-9593-817d4086c09b"), 400m, 800m, new DateTimeOffset(new DateTime(2025, 2, 17, 17, 56, 2, 709, DateTimeKind.Unspecified).AddTicks(5163), new TimeSpan(0, 0, 0, 0, 0)), "USDT", null, 0.15m, new DateTimeOffset(new DateTime(2025, 2, 17, 17, 56, 2, 709, DateTimeKind.Unspecified).AddTicks(5164), new TimeSpan(0, 0, 0, 0, 0)) }
                });
        }
    }
}
