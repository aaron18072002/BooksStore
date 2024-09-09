using BooksStore.DataAccess.Database;
using BooksStore.DataAccess.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            this._db.Products.Include(p => p.Category).Include(p => p.CategoryId);
        }

        public async Task<T> Add(T entity)
        {
            this.DbSet.Add(entity);
            await this._db.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> GetAll(string? includeProperties = null)
        {
            IQueryable<T> result = this.DbSet;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    result = result.Include(includeProp);
                }
            }

            return await result.ToListAsync();
        }

        public async Task<T?> GetDetails
            (Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> result;
            if(tracked)
            {
                result = this.DbSet;
            }
            else
            {
                result = this.DbSet.AsNoTracking();
            }

            result = result.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    result = result.Include(includeProp);
                }
            }

            return await result.FirstOrDefaultAsync();
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
