using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MenuItemServices.Dtos.Ingredients
{
    public class AddingIngredientItemToStockRequest
    {
        public int IngredientId {get;set;}
        public int CurrentQuantity {get;set;}
    }
    public class AddingIngredientToStockRequest{
        public ICollection<AddingIngredientItemToStockRequest>  addingIngredientItemToStockRequests {get;set;} = [];
        [AllowNull]
        public DateTime ? ArrivedDate {get;set;}
    }
}