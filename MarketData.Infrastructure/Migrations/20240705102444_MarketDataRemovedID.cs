using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MarketData.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MarketDataRemovedID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MarketData",
                table: "MarketData");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MarketData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MarketData",
                table: "MarketData",
                columns: new[] { "TimeUtc", "Asset" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MarketData",
                table: "MarketData");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "MarketData",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MarketData",
                table: "MarketData",
                column: "Id");
        }
    }
}
