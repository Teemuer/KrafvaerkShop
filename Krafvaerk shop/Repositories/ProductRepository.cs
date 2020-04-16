using Krafvaerk_shop.Data;
using Krafvaerk_shop.DBModels;
using Krafvaerk_shop.Interfaces;
using Krafvaerk_shop.Models;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Krafvaerk_shop.Repositories
{
    public class ProductRepository : RepositoryBase<Product, ShopDBContext>, IProductRepository
    {
        public ProductRepository(ShopDBContext shopDBContext) : base(shopDBContext)
        {
        }

        public PageProduct GetPageProduct(int id)
        {
            Product dbProduct = _shopDBContext.Product.FirstOrDefault(x => x.Id == id);
            if (dbProduct != null)
            {
                //lets try to get media file for product
                dbProduct.Media = _shopDBContext.Media.FirstOrDefault(x => x.Id == dbProduct.MediaId);
                return new PageProduct(dbProduct);
            }
            return null;
        }

        public List<PageProduct> SelectNewestPageProducts(int take)
        {
            IQueryable<Product> prods = _shopDBContext.Product.OrderByDescending(x => x.Created)?.Take(take).Include(nameof(Media));
            return MapProductsToPageProducts(prods);
        }

        List<PageProduct> IProductRepository.SelectPageProducts()
        {
            IQueryable<Product> prods = SelectAll().Include(nameof(Media));
            return MapProductsToPageProducts(prods);
        }

        List<PageProduct> IProductRepository.SelectPageProductsByCondition(Expression<Func<Product, bool>> expression)
        {
            IQueryable<Product> prods = SelectByCondition(expression).Include(nameof(Media));
            return MapProductsToPageProducts(prods);
        }

        private static List<PageProduct> MapProductsToPageProducts(IQueryable<Product> prods)
        {
            List<PageProduct> products = new List<PageProduct>();
            if(prods != null)
            {
                foreach (var item in prods)
                {
                    products.Add(new PageProduct(item));
                }
            }
            return products;
        }
    }
}
