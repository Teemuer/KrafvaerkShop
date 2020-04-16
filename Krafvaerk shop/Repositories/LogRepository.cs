using Krafvaerk_shop.Data;
using Krafvaerk_shop.DBModels;
using Krafvaerk_shop.Interfaces;

namespace Krafvaerk_shop.Repositories
{
    public class LogRepository : RepositoryBase<Log, ShopDBContext>, ILog
    {
        public LogRepository(ShopDBContext shopDBContext) : base(shopDBContext)
        {
        }
    }
}
