using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Dtos.Ingredients
{
    public class ItemQuantityIngredientResponse
    {
        public int IngredientId {get;set;}
        public string IngredientName {get;set;}
        public int RecommendedUsedQuantity {get;set;}
        public int MaxQuantity {get;set;}
        public int MinQuantity {get;set;}
    }
}