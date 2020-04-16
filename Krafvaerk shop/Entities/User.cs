using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Krafvaerk_shop.DBModels
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Required]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [MaxLength(100)]
        [Required]
        public string Country { get; set; }

        [MaxLength(200)]
        [Required]
        public string Address { get; set; }

        [MaxLength(12)]
        [Required]
        [DisplayName("Post code")]
        public string PostCode { get; set; }
        
        [Required]
        [Phone]
        [DisplayName("Phone")]
        public string PhoneNumber { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
