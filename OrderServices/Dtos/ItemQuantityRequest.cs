using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Dtos.Ingredients
{
    public class ItemQuantityRequest
    {
        public int ItemId {get;set;}
        public int ItemQuantity {get;set;}
    }

}