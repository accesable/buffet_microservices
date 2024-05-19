using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Ingredients;

namespace Shared.MenuItem
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        [Required]
        [StringLength(128)]
        public string ItemName { get; set; } = null!;
        [Required]
        [StringLength(255)]
        public string ItemDescription { get; set; } = null!;
        [Required]
        public decimal OriginalPrice { get; set; }
        public virtual ICollection<Image> Images { get; set; } = new List<Image>();
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        public bool IsLocked { get; set; } = false;
        public bool IsCharged { get; set; } = false;
        public virtual ICollection<Item_Ingredient> Item_Ingredients {get;set;} = new List<Item_Ingredient>();
    }
}
