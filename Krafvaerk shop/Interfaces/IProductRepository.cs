using Krafvaerk_shop.DBModels;
using Krafvaerk_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Krafvaerk_shop.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        List<PageProduct> SelectPageProductsByCondition(Expression<Func<Product, bool>> expression);
        List<PageProduct> SelectPageProducts();
        List<PageProduct> SelectNewestPageProducts(int take);
        PageProduct GetPageProduct(int id);
    }
}
