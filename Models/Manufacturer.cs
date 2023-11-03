using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
