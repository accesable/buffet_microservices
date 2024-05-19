using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.MenuItem
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(128)]
        public string CategoryName { get; set; } = null!;
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get;set; } = DateTime.UtcNow;
    }
}
