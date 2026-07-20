using Northwind.EntityModels;

namespace Northwind.WebApi.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> CreateAsync(Customer c, CancellationToken token);
    Task<Customer[]> RetrieveAllAsync(CancellationToken token);
    Task<Customer?> RetrieveAsync(string id, CancellationToken token);
    Task<Customer?> UpdateAsync(Customer c, CancellationToken token);
    Task<bool> DeleteAsync(string id, CancellationToken token);
}
