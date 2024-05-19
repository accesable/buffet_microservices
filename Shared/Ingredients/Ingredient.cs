using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Ingredients
{
    public class Ingredient
    {
        [Key]
        public int IngredientId {get;set;}
        [StringLength(128)]
        public string IngredientName {get;set;} = null!;
        [StringLength(255)]
        public string IngredientDescription {get;set;} = null!;
        [StringLength(128)]
        public string UnitOfMeasurement {get;set;} = null!;
        public TimeSpan ExpiredTime {get;set;}
        public virtual ICollection<Item_Ingredient> Item_Ingredients {get;set;} = new List<Item_Ingredient>();
        public virtual ICollection<Ingredient_Stock> Ingredient_Stocks {get;set;} = new List<Ingredient_Stock>();
    }
}