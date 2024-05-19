using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Ingredients
{
    public class Ingredient_Stock
    {
        [Key]
        public int Id {get;set;}
        public int IngredientId {get;set;}
        public int StockId {get;set;}
        public int CurrentQuantity {get;set;}
        [AllowNull]
        public DateTime ? ExpiredAt {get;set;}
        [ForeignKey("IngredientId")]
        public virtual Ingredient Ingredient {get;set;} = null!;
        [ForeignKey("StockId")]
        public virtual Stock Stock {get;set;} = null!;
        public virtual ICollection<Order_IngredientStock> Order_IngredientStocks {get;set;} = new List<Order_IngredientStock>();
    }
}