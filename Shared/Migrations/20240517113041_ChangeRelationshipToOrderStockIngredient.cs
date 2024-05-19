using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationshipToOrderStockIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetail_IngredientStocks");

            // migrationBuilder.RenameColumn(
            //     name: "MinQuantity",
            //     table: "Item_Ingredients",
            //     newName: "MinQuantityPerItem");

            // migrationBuilder.RenameColumn(
            //     name: "MaxQuantity",
            //     table: "Item_Ingredients",
            //     newName: "MaxQuantityPerItem");

            // migrationBuilder.AlterColumn<DateTime>(
            //     name: "ExpiredAt",
            //     table: "Ingredient_Stocks",
            //     type: "datetime(6)",
            //     nullable: true,
            //     oldClrType: typeof(DateTime),
            //     oldType: "datetime(6)");

            migrationBuilder.CreateTable(
                name: "Order_IngredientStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    IngredientStockId = table.Column<int>(type: "int", nullable: false),
                    UsedQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_IngredientStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_IngredientStocks_Ingredient_Stocks_IngredientStockId",
                        column: x => x.IngredientStockId,
                        principalTable: "Ingredient_Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_IngredientStocks_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IngredientStocks_IngredientStockId",
                table: "Order_IngredientStocks",
                column: "IngredientStockId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_IngredientStocks_OrderId",
                table: "Order_IngredientStocks",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order_IngredientStocks");

            // migrationBuilder.RenameColumn(
            //     name: "MinQuantityPerItem",
            //     table: "Item_Ingredients",
            //     newName: "MinQuantity");

            // migrationBuilder.RenameColumn(
            //     name: "MaxQuantityPerItem",
            //     table: "Item_Ingredients",
            //     newName: "MaxQuantity");

            // migrationBuilder.AlterColumn<DateTime>(
            //     name: "ExpiredAt",
            //     table: "Ingredient_Stocks",
            //     type: "datetime(6)",
            //     nullable: false,
            //     defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            //     oldClrType: typeof(DateTime),
            //     oldType: "datetime(6)",
            //     oldNullable: true);

            migrationBuilder.CreateTable(
                name: "OrderDetail_IngredientStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IngredientStockId = table.Column<int>(type: "int", nullable: false),
                    OrderDetailId = table.Column<int>(type: "int", nullable: false),
                    UsedQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail_IngredientStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_IngredientStocks_Ingredient_Stocks_IngredientSto~",
                        column: x => x.IngredientStockId,
                        principalTable: "Ingredient_Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_IngredientStocks_OrderDetails_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "OrderDetails",
                        principalColumn: "OrderDetailId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_IngredientStocks_IngredientStockId",
                table: "OrderDetail_IngredientStocks",
                column: "IngredientStockId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_IngredientStocks_OrderDetailId",
                table: "OrderDetail_IngredientStocks",
                column: "OrderDetailId");
        }
    }
}
