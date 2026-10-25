using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Captain.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCategoryToUseCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_transaction_category_category_id",
                schema: "money",
                table: "transaction");

            migrationBuilder.DropIndex(
                name: "ix_transaction_category_id",
                schema: "money",
                table: "transaction");

            migrationBuilder.DropPrimaryKey(
                name: "pk_category",
                schema: "money",
                table: "category");

            migrationBuilder.AddPrimaryKey(
                name: "pk_category",
                schema: "money",
                table: "category",
                columns: new[] { "id", "app_user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_transaction_category_id_app_user_id",
                schema: "money",
                table: "transaction",
                columns: new[] { "category_id", "app_user_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_transaction_category_category_id_app_user_id",
                schema: "money",
                table: "transaction",
                columns: new[] { "category_id", "app_user_id" },
                principalSchema: "money",
                principalTable: "category",
                principalColumns: new[] { "id", "app_user_id" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_transaction_category_category_id_app_user_id",
                schema: "money",
                table: "transaction");

            migrationBuilder.DropIndex(
                name: "ix_transaction_category_id_app_user_id",
                schema: "money",
                table: "transaction");

            migrationBuilder.DropPrimaryKey(
                name: "pk_category",
                schema: "money",
                table: "category");

            migrationBuilder.AddPrimaryKey(
                name: "pk_category",
                schema: "money",
                table: "category",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_category_id",
                schema: "money",
                table: "transaction",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "fk_transaction_category_category_id",
                schema: "money",
                table: "transaction",
                column: "category_id",
                principalSchema: "money",
                principalTable: "category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
