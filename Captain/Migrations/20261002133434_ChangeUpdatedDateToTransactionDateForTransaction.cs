using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captain.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUpdatedDateToTransactionDateForTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_date",
                schema: "money",
                table: "transaction",
                newName: "transaction_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "transaction_date",
                schema: "money",
                table: "transaction",
                newName: "updated_date");
        }
    }
}
