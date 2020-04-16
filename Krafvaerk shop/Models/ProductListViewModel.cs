using Krafvaerk_shop.DBModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Krafvaerk_shop.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<PageProduct> Products { get; set; }
        public IEnumerable<ProductCategory> Categories { get; set; }
    }
}
