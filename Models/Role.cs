using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public List<Buyer> Users { get; set; } = new List<Buyer>();
    }
}
