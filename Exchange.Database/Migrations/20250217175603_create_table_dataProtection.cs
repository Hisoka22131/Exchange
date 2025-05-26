using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Exchange.Database.Migrations
{
    /// <inheritdoc />
    public partial class create_table_dataProtection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: new Guid("8708e429-96b6-4259-b23c-51ed417d607d"));

            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: new Guid("99568b23-ff26-467f-8a1c-c86aae74092b"));

            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: new Guid("d995e9f3-770b-4e81-8fa5-2edd6ad870d2"));

            migrationBuilder.EnsureSchema(
                name: "security");

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FriendlyName = table.Column<string>(type: "text", nullable: true),
                    Xml = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                schema: "security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FriendlyName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Xml = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "DataProtectionKeys",
                schema: "security");

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

            migrationBuilder.InsertData(
                table: "Commissions",
                columns: new[] { "Id", "AmountFrom", "AmountTo", "CreatedAt", "Currency", "FixedFee", "PercentFee", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("8708e429-96b6-4259-b23c-51ed417d607d"), 0m, 400m, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9642), new TimeSpan(0, 0, 0, 0, 0)), "USDT", 20m, 0m, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9700), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("99568b23-ff26-467f-8a1c-c86aae74092b"), 400m, 800m, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9708), new TimeSpan(0, 0, 0, 0, 0)), "USDT", null, 0.15m, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9710), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d995e9f3-770b-4e81-8fa5-2edd6ad870d2"), 800m, null, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9713), new TimeSpan(0, 0, 0, 0, 0)), "USDT", null, 0.05m, new DateTimeOffset(new DateTime(2025, 2, 11, 19, 21, 41, 311, DateTimeKind.Unspecified).AddTicks(9714), new TimeSpan(0, 0, 0, 0, 0)) }
                });
        }
    }
}
