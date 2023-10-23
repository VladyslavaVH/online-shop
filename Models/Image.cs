using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Image
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Url]
        public string Url { get; set; }

        [Required]
        [Column("ProductFK")]
        public int ProductFK { get; set; }

        [ForeignKey("ProductFK")]
        public virtual Product Product { get; set; }
    }
}
