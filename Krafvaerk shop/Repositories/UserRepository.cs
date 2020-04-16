using Krafvaerk_shop.Data;
using Krafvaerk_shop.DBModels;
using Krafvaerk_shop.Interfaces;

namespace Krafvaerk_shop.Repositories
{
    public class UserRepository : RepositoryBase<User, ShopDBContext>, IUserRepository
    {
        public UserRepository(ShopDBContext shopDBContext) : base(shopDBContext)
        {
        }
    }
}
