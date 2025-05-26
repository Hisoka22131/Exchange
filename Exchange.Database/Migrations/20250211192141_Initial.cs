using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Exchange.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    AmountFrom = table.Column<decimal>(type: "numeric", nullable: false),
                    AmountTo = table.Column<decimal>(type: "numeric", nullable: true),
                    FixedFee = table.Column<decimal>(type: "numeric", nullable: true),
                    PercentFee = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TelegramUserId = table.Column<long>(type: "bigint", nullable: false),
                    TelegramUserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyFrom = table.Column<string>(type: "text", nullable: false),
                    CurrencyTo = table.Column<string>(type: "text", nullable: false),
                    AmountFrom = table.Column<decimal>(type: "numeric(18,9)", nullable: false),
                    AmountTo = table.Column<decimal>(type: "numeric(18,9)", nullable: false),
                    Commission = table.Column<decimal>(type: "numeric(18,9)", nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PhoneNumberUser = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CryptoNetworkCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CryptoNetworkName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FiatNetworkCode = table.Column<string>(type: "text", nullable: false),
                    FiatNetworkName = table.Column<string>(type: "text", nullable: false),
                    WalletAddressUser = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    WalletAddressAdmin = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Commissions",
                columns: new[] { "Id", "AmountFrom", "AmountTo", "CreatedAt", "Currency", "FixedFee", "PercentFee", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("8708e429-96b6-4259-b23c-51ed417d607d"), 0m, 400m, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9642), new TimeSpan(0, 0, 0, 0, 0)), "USDT", 20m, 0m, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9700), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("99568b23-ff26-467f-8a1c-c86aae74092b"), 400m, 800m, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9708), new TimeSpan(0, 0, 0, 0, 0)), "USDT", null, 0.15m, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9710), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d995e9f3-770b-4e81-8fa5-2edd6ad870d2"), 800m, null, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9713), new TimeSpan(0, 0, 0, 0, 0)), "USDT", null, 0.05m, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9714), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commissions");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
