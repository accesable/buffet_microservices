using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Shared.OrderModels;

namespace Shared.Ingredients
{
    [Table(name:"Order_IngredientStocks")]
    public class Order_IngredientStock
    {
        [Key]
        public int Id {get;set;}
        public int OrderId {get;set;}
        public int IngredientStockId {get;set;}
        public int UsedQuantity {get;set;}
        [ForeignKey("OrderId")]
        public virtual Order Order {get;set;} = null!;
        [ForeignKey("IngredientStockId")]
        public virtual Ingredient_Stock Ingredient_Stock {get;set;} = null!;
    }
}