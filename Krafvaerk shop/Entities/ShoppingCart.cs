using System;
using System.ComponentModel.DataAnnotations;

namespace Krafvaerk_shop.DBModels
{
    public class ShoppingCart
    {
        [Key]
        public string Id { get; set; }
        
        [MaxLength(1000)]
        public string CartDataJSON { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        public DateTime Expires { get; set; }
    }
}
