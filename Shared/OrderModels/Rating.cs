using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.OrderModels
{
    public class Rating
    {
        [Key]
        public int RatingId {get;set;}
        public int Star {get;set;}
        public string ? Comment {get;set;}
    }
}