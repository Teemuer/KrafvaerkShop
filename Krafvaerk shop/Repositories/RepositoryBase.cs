using Krafvaerk_shop.Data;
using Krafvaerk_shop.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Krafvaerk_shop.Repositories
{
    public abstract class RepositoryBase<T, TContext> : IRepositoryBase<T> 
        where T : class
        where TContext : DbContext
    {
        protected ShopDBContext _shopDBContext { get; set; }

        public RepositoryBase(ShopDBContext shopDBContext)
        {
            _shopDBContext = shopDBContext;
        }

        public void Create(T entity)
        {
            _shopDBContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
           _shopDBContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> SelectAll()
        {
            return _shopDBContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> SelectByCondition(Expression<Func<T, bool>> expression)
        {
            return _shopDBContext.Set<T>().Where(expression).AsNoTracking();
        }

        public T GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _shopDBContext.Set<T>().FirstOrDefault(expression);
        }

        public void Update(T entity)
        {
            _shopDBContext.Set<T>().Update(entity);
        }
    }
}
