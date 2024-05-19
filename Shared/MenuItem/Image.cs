using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.MenuItem
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; } // Primary key
        [Required]
        [StringLength(128)]
        public string ImageUrl { get; set; } = null!; // Property to store the URL of the image
        public int ItemId { get; set; } // Foreign key property to associate with the Item

        [ForeignKey("ItemId")] // Foreign key attribute to associate with the Item
        public Item Item { get; set; } = null!; // Navigation property back to the Item
    }
}
