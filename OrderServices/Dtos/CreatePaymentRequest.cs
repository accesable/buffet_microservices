using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shared.PaymentModels;

namespace OrderServices.Dtos
{
        public class CreatePaymentRequest
        {
            [Required]
            public double Ammount {get;set;}
            public PaymentMethod PaymentMethod {get;set;} = PaymentMethod.CASH;
            public PaymentStatus PaymentStatus {get;set;} = PaymentStatus.PENDING;
        }
}