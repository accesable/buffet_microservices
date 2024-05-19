using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Shared.OrderModels;

namespace OrderServices.Dtos
{
    public class UpdateDetailStatusRequest
    {
        [Required]
        public int OrderDetailId {get;set;}
        [Required]
        public DetailStatus Status {get;set;}
    }
}