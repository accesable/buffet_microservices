using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Mysqlx.Crud;

namespace Shared.PaymentModels
{
    public class Payment
    {
        [Key]
        public int PaymentId {get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get;set; } = DateTime.UtcNow;
        public double Ammount {get;set;} 
        public PaymentMethod PaymentMethod {get;set;} = PaymentMethod.CASH;
        public PaymentStatus Status {get;set;} = PaymentStatus.PENDING;
    }
}