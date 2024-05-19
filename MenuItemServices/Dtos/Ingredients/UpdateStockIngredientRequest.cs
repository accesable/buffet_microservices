using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuItemServices.Dtos.Ingredients
{
    public class UpdateStockIngredientRequest
    {
        public int IngredientId {get;set;}
        public int UpdateQuantity {get;set;}
    }
    public class UpdateStockIngredientQuantityRequest
    {
        public int StockId {get;set;}
        public IList<UpdateStockIngredientRequest> IngredientQuantities  {get;set;}
    }
}