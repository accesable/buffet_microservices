using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Authentication;
using Shared.Ingredients;
using Shared.PaymentModels;

namespace Shared.OrderModels
{
    public class Order
    {
        [Key]
        public int OrderId {get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get;set; } = DateTime.UtcNow;
        public int NumberOfCustomers {get;set;}
        public OrderStatus Status {get;set;}
        public int BTableId {get;set;}
        [ForeignKey("BTableId")]
        public virtual BTable Table {get;set;} =null!;
        public virtual List<OrderDetail> OrderDetails {get;set;} = [];
        public string EmployeeId {get;set;} = null!;
        [ForeignKey("EmployeeId")]
        public virtual AppUser Employee {get;set;} =null!;
        public virtual List<Order_IngredientStock> Order_IngredientStocks {get;set;} = [];
        [AllowNull]
        public int PaymentId {get;set;}
        [ForeignKey("PaymentId")]
        public virtual Payment ? Payment {get;set;}
        [AllowNull]
        public int RatingId {get;set;}
        [ForeignKey("RatingId")]
        public virtual Rating ? Rating {get;set;}
    }
}