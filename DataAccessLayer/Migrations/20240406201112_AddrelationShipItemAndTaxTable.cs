using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddrelationShipItemAndTaxTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxItems_Taxes_TaxId",
                table: "TaxItems");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxItems_Taxes_TaxId",
                table: "TaxItems",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxItems_Taxes_TaxId",
                table: "TaxItems");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxItems_Taxes_TaxId",
                table: "TaxItems",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "Id");
        }
    }
}
