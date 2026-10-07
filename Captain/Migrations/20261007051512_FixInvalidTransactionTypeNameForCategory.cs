using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captain.Migrations
{
    /// <inheritdoc />
    public partial class FixInvalidTransactionTypeNameForCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "transacion_type",
                schema: "money",
                table: "category",
                newName: "transaction_type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "transaction_type",
                schema: "money",
                table: "category",
                newName: "transacion_type");
        }
    }
}
