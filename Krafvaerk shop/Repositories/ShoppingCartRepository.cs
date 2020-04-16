using Krafvaerk_shop.Data;
using Krafvaerk_shop.DBModels;
using Krafvaerk_shop.Interfaces;

namespace Krafvaerk_shop.Repositories
{
    public class ShoppingCartRepository : RepositoryBase<ShoppingCart, ShopDBContext>, IShoppingCartRepository
    {
        public ShoppingCartRepository(ShopDBContext shopDBContext) : base(shopDBContext)
        {
        }
    }
}
