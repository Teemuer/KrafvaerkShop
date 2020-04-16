using System;
using System.Linq;
using System.Linq.Expressions;

namespace Krafvaerk_shop.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> SelectAll();
        IQueryable<T> SelectByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetByCondition(Expression<Func<T, bool>> expression);
    }
}
