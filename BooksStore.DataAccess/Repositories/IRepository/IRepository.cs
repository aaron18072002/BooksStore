using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T? GetDetails(Expression<Func<T, bool>> filter);

        T Add(T entity);

        bool Remove(T entity);

        bool RemoveRange(IEnumerable<T> entities);
    }
}
