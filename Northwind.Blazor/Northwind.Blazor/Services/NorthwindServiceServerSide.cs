using Microsoft.EntityFrameworkCore;
using Northwind.DataContext.Sqlite;
using Northwind.EntityModels;

namespace Northwind.Blazor.Services;

public class NorthwindServiceServerSide(NorthwindContext db) : INorthwindService
{
    private readonly NorthwindContext _db = db;
    public Task<Customer> CreateCustomerAsync(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        _db.Customers.Add(customer);
        _db.SaveChangesAsync();
        return Task.FromResult(customer);
    }

    public Task DeleteCustomerAsync(string customerId)
    {
        ArgumentNullException.ThrowIfNull(customerId);

        Customer? existCustomer = _db.Customers.Include(c => c.Orders).FirstOrDefaultAsync(c => c.CustomerId == customerId).Result;

        if (existCustomer is not null)
        {
            _db.RemoveRange(existCustomer.Orders);
            _db.Customers.Remove(existCustomer);
            return _db.SaveChangesAsync();
        }
        return Task.CompletedTask;
    }

    public Task<Customer?> GetCustomerAsync(string customerId)
    {
        ArgumentNullException.ThrowIfNull(customerId);

        return _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public Task<List<Customer>> GetCustomersAsync()
    {
        return _db.Customers.ToListAsync();
    }

    public Task<List<Customer>> GetCustomersAsync(string country)
    {
        ArgumentNullException.ThrowIfNull(country);

        return _db.Customers.Where(c => c.Country == country).ToListAsync();
    }

    public Task<Customer> UpdateCustomerAsync(Customer customer)
    {
        _db.Entry(customer).State = EntityState.Modified;
        _db.SaveChangesAsync();
        return Task.FromResult(customer);
    }
}
