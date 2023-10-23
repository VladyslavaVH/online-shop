using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Color { get; set; }

        [Column("CategoryFK")]
        public int CategoryFK { get; set; }

        [ForeignKey("CategoryFK")]
        public virtual Categories Category { get; set; }

        [Column("ManufacturerFK")]
        public int ManufacturerFK { get; set; }

        [ForeignKey("ManufacturerFK")]
        public virtual Manufacturer Manufacturer { get; set; }

        public string Description { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public ICollection<Image> Images { get; set; }
        public ICollection<Buyer> Buyers { get; set; }
        public List<Order> Orders { get; set; }
    }
}
