using BooksStore.DataAccess.Database;
using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.Repositories
{
	public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
	{
		public OrderHeaderRepository(BooksStoreDbContext db) : base(db)
		{

		}
		public Task Update(OrderHeader orderHeader)
		{
			throw new NotImplementedException();
		}
	}
}
