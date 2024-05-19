using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuItemServices.Dtos.Ingredients
{
    public class IngredientRequest
    {

        public string IngredientName {get;set;} = null!;
        public string IngredientDescription {get;set;} = null!;
        public string UnitOfMeasurement {get;set;} = null!;
        public double ExpiredTimeInHours { get; set; } // Represent TimeSpan in hours
        public double ExpiredTimeInDays { get; set; } // Represent TimeSpan in days
    }
}