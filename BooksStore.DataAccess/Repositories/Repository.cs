using BooksStore.DataAccess.Database;
using BooksStore.DataAccess.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly BooksStoreDbContext _db;
        internal DbSet<T> DbSet;
        public Repository(BooksStoreDbContext db)
        {
            this._db = db;
            this.DbSet = this._db.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            this.DbSet.Add(entity);
            await this._db.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var result = await this.DbSet.ToListAsync();

            return result;
        }

        public async Task<T?> GetDetails(Expression<Func<T, bool>> filter)
        {
            var result = await this.DbSet.FirstOrDefaultAsync(filter);

            return result;
        }

        public async Task<bool> Remove(T entity)
        {
            
            this.DbSet.Remove(entity);
            await this._db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveRange(IEnumerable<T> entities)
        {
            this.DbSet.RemoveRange(entities);
            await this._db.SaveChangesAsync();

            return true;
        }
    }
}
