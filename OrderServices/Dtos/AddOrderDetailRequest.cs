using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Shared.OrderModels;

namespace OrderServices.Dtos
{
    public class OrderDetailRequest
    {
        [Required]
        public int ItemId {get;set;}
        [AllowNull]
        public string ? DetailNote {get;set;}
        public int Quantity {get;set;}
    }
    public class AddOrderDetailRequest
    {
        [Required]
        public int OrderId {get;set;}
        public ICollection<OrderDetailRequest> orderDetailRequests {get;set;} = new List<OrderDetailRequest>();
    }
}