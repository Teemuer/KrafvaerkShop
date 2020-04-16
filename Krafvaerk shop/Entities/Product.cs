using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Krafvaerk_shop.DBModels
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Vat { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual ProductCategory Category { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(MediaId))]
        public virtual Media Media { get; set; }
        public int MediaId { get; set; }

    }
}
