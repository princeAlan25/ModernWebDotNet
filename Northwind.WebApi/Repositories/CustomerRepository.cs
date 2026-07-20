using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Hybrid;
using Northwind.DataContext.Sqlite;
using Northwind.EntityModels;

namespace Northwind.WebApi.Repositories
{
    public class CustomerRepository(HybridCache cache, NorthwindContext db) : ICustomerRepository
    {
        private readonly HybridCache _cache = cache;
        private readonly NorthwindContext _db = db;
        public async Task<Customer?> CreateAsync(Customer c, CancellationToken token)
        {
            var result = await _db.Customers.AddAsync(c, token);
            int affectedRow = await _db.SaveChangesAsync(cancellationToken: token);
            if(affectedRow == 1)
            {
                await _cache.SetAsync(result.Entity.CustomerId, c);
                return c;
            }
            return null;

        }

        public async Task<bool> DeleteAsync(string id, CancellationToken token)
        {
            Customer? customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id, token);
            if(customer is not null)
            {
                _db.Remove(customer);
                int affectedRow = await _db.SaveChangesAsync(token);
                if (affectedRow == 1) await _cache.RemoveAsync(customer.CustomerId, token);
                return true;
            }
            return false;
        }

        public Task<Customer[]> RetrieveAllAsync(CancellationToken token)
        {
            return _db.Customers.ToArrayAsync(token);
        }

        public async Task<Customer?> RetrieveAsync(string id, CancellationToken token)
        {
            return await _cache.GetOrCreateAsync(
                key: id,
                factory: async cancel => await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id, cancel),
                cancellationToken: token
                );
        }

        public async Task<Customer?> UpdateAsync(Customer c, CancellationToken token)
        {
            _db.Update(c);
            int affectedRow = await _db.SaveChangesAsync(token);
            if (affectedRow == 1) 
            {
                await _cache.SetAsync(c.CustomerId, c);
                return c;
            }
            return null;
        }
    }
}
