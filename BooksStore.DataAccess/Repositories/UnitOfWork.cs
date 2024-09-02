﻿using BooksStore.DataAccess.Database;
using BooksStore.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BooksStoreDbContext _db;
        public ICategoryRepository Categories { get; private set; }
        public UnitOfWork(BooksStoreDbContext db)
        {
            this._db = db;
            this.Categories = new CategoryRepository(db);
        }

        public async Task Save()
        {
            await this._db.SaveChangesAsync();
        }
    }
}
