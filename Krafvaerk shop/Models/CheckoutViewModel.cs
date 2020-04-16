using Krafvaerk_shop.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Krafvaerk_shop.ShoppingCartApiController;

namespace Krafvaerk_shop.Models
{
    public class CheckoutViewModel
    {
        public CheckoutViewModel()
        {
            Products = new List<ShoppingCartProductDTO>();
        }
        public CheckoutViewModel(ShoppingCart cart)
        {
            if (!string.IsNullOrEmpty(cart.CartDataJSON))
            {
                Products = JsonSerializer.Deserialize<List<ShoppingCartProductDTO>>(cart.CartDataJSON);
            }
            ShoppingCartId = cart.Id;
        }
        public string ShoppingCartId { get; set; }
        public List<ShoppingCartProductDTO> Products { get; set; }
        public User User { get; set; }

        [DisplayName("Total price")]
        public string TotalPrice => Products?.Sum(x => x.Price * x.Quantity).ToString("N2");

        [Required]
        [DisplayName("Has consent")]
        public bool HasConsent { get; set; }
    }
}
