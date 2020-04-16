using Krafvaerk_shop.Data;
using Krafvaerk_shop.Interfaces;

namespace Krafvaerk_shop.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ShopDBContext _shopDbContext;
        public RepositoryWrapper(ShopDBContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }

        private IProductRepository _productRepository;
        public IProductRepository IProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_shopDbContext);
                }
                return _productRepository;
            }
        }
        private IShoppingCartRepository _shoppingCartRepository;
        public IShoppingCartRepository IShoppingCartRepository
        {
            get
            {
                if (_shoppingCartRepository == null)
                {
                    _shoppingCartRepository = new ShoppingCartRepository(_shopDbContext);
                }
                return _shoppingCartRepository;
            }
        }
        private ILog _iLog;
        public ILog ILog
        {
            get
            {
                if (_iLog == null)
                {
                    _iLog = new LogRepository(_shopDbContext);
                }
                return _iLog;
            }
        }
        private IUserRepository _userRepository;
        public IUserRepository IUserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_shopDbContext);
                }
                return _userRepository;
            }
        }
        private IOrderRepository _orderRepository;
        public IOrderRepository IOrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_shopDbContext);
                }
                return _orderRepository;
            }
        }

        public void Save()
        {
            _shopDbContext.SaveChanges();
        }
    }
}
