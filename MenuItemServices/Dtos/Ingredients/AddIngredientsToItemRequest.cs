using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MenuItemServices.Dtos.Ingredients
{

    public class AddingIngredientToItemRequest
    {
        public class IngredientItemRequest
        {
            public int IngredientId {get;set;}
            public int MaxQuantity {get;set;}
            public int MinQuantity {get;set;}
        }
        [Required]
        public int ItemId {get;set;}
        [Required]
        public List<IngredientItemRequest> IngredientItemRequests {get;set;} = [];
    }
}