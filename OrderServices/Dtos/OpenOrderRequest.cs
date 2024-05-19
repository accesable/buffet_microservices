using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Dtos
{
    public class OpenOrderRequest
    {
        [Required]
        public int TableId {get;set;}
        [Required]
        public int NumberOfCustomer {get;set;}
        public string ? EmployeeId {get;set;}
    }
}