
using System.Collections.Generic;

namespace Krafvaerk_shop.Models
{
    public class PageProduct
    {
        public PageProduct(DBModels.Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price; 
            Description = product.Description;
            ImageUri = product.Media?.Path;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUri { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        private const int _maxDescriptionLength = 50;
        public string ShortDescription
        {
            get
            {
                return Description.Length > _maxDescriptionLength ? Description.Substring(0, _maxDescriptionLength) : Description;
            }
        }

    }
}
