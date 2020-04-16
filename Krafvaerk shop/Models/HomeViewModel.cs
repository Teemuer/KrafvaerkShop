using System;
using System.Collections.Generic;

namespace Krafvaerk_shop.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            NewestProducts = new List<PageProduct>();
        }

        public HomeViewModel(List<PageProduct> newestProducts)
        {
            NewestProducts = newestProducts;
        }

        public List<PageProduct> NewestProducts { get; set; }
    }
}
