using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Shared.Ingredients;
using Shared.MenuItem;

namespace Shared.OrderModels
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId {get;set;}
        public int Quantity {get;set;}
        public DetailStatus DetailStatus {get;set;}
        public int ItemId {get;set;}
        [ForeignKey("ItemId")]
        public virtual Item Item {get;set;} = null!;
        public int OrderId {get;set;}
        public virtual Order Order {get;set;} =  null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get;set; } = DateTime.UtcNow;
        public string DetailNote {get;set;} = null!;
    }
}