using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Shared.MenuItem;

namespace Shared.Ingredients
{
    public class Item_Ingredient
    {
        [Key]
        public int Id {get;set;}
        public int IngredientId {get;set;}
        public int ItemId {get;set;}
        public int MaxQuantity {get;set;}
        public int MinQuantity {get;set;}
        [ForeignKey("ItemId")]
        public virtual Item Item {get;set;} = null!;
        [ForeignKey("IngredientId")]
        public virtual Ingredient Ingredient {get;set;} = null!;
    }
}