using Krafvaerk_shop.Data;
using Krafvaerk_shop.DBModels;
using Krafvaerk_shop.Interfaces;

namespace Krafvaerk_shop.Repositories
{
    public class OrderRepository : RepositoryBase<Order, ShopDBContext>, IOrderRepository
    {
        public OrderRepository(ShopDBContext shopDBContext) : base(shopDBContext)
        {
        }
    }
}
