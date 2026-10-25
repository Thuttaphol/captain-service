using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captain.Migrations
{
    /// <inheritdoc />
    public partial class TransactionUpdatedDateFromWithoutTimeZoneToWithTimeZone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_date",
                schema: "money",
                table: "transaction",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_date",
                schema: "money",
                table: "transaction",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
