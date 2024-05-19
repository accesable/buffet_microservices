using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.OrderModels
{
    public class BTable
    {
        [Key]
        public int BTableId {get;set;}
        public BTableStatus Status {get;set;}
        public int Capacity {get;set;}
    }
}