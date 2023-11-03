using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Models
{
    public class Buyer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [StringLength(13)]
        [Phone(ErrorMessage = "Invalid phone")]
        public string Phone { get; set; }

        [StringLength(30)]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Column("RoleFK")]
        public int? RoleFK { get; set; }

        [ForeignKey("RoleFK")]
        public virtual Role Role { get; set; }

        [Required]
        public ICollection<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
    }
}
