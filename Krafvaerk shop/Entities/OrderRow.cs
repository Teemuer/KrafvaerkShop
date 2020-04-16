using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Krafvaerk_shop.DBModels
{
    [Table("OrderRow")]
    public class OrderRow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        
        [Required]
        [MinLength(1)]
        public int Quantity { get; set; }
    }
}
