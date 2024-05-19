using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Dtos
{
    public class AvailableIngredientResponse
    {
        public int Id {get;set;}
        public int IngredientId {get;set;}
        public string  IngredientName {get;set;}
        public int CurrentQuantity {get;set;}
        public int StockId {get;set;}
        public DateTime ? ExpriedDate {get;set;} 
        public string  Status {get;set;}
    }
}