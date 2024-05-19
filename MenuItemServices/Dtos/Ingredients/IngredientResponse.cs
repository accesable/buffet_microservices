using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuItemServices.Dtos.Ingredients
{
    public class IngredientResponse
    {
        public int IngredientId {get;set;}
        public string IngredientName {get;set;} = null!;
        public string IngredientDescription {get;set;} = null!;
        public string UnitOfMeasurement {get;set;} = null!;
        public TimeSpan ExpiredTime {get;set;}
    }
}