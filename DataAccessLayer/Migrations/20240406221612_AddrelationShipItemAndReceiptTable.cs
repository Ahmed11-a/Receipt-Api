using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddrelationShipItemAndReceiptTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiptId",
                table: "itemDatas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_itemDatas_ReceiptId",
                table: "itemDatas",
                column: "ReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_itemDatas_Receipts_ReceiptId",
                table: "itemDatas",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itemDatas_Receipts_ReceiptId",
                table: "itemDatas");

            migrationBuilder.DropIndex(
                name: "IX_itemDatas_ReceiptId",
                table: "itemDatas");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                table: "itemDatas");
        }
    }
}
