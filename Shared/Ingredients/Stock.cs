using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Ingredients
{
    public class Stock
    {
        [Key]
        public int StockId {get;set;}
        public int NumberOfIngredient {get;set;}
        public DateTime ArrivedDate {get;set;}
        public virtual ICollection<Ingredient_Stock> Ingredient_Stocks {get;set;} = new List<Ingredient_Stock>();
    }
}