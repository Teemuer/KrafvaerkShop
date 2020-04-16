using Krafvaerk_shop.DBModels;
using Krafvaerk_shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Krafvaerk_shop.Interfaces
{
    public interface IRepositoryWrapper
    {
        IProductRepository IProductRepository { get; }
        IShoppingCartRepository IShoppingCartRepository { get; }
        IOrderRepository IOrderRepository { get; }
        IUserRepository IUserRepository { get; }
        ILog ILog { get; }

        void Save();
    }
}
