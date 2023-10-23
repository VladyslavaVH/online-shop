using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Order
    {
        [Column("BuyerFK")]
        public int BuyerFK { get; set; }

        [ForeignKey("BuyerFK")]
        public Buyer Buyer { get; set; }

        [Column("ProductFK")]
        public int ProductFK { get; set; }

        [ForeignKey("ProductFK")]
        public Product Product { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateTime { get; set; }
    }
}
