using Northwind.WebApi.Repositories;
using Northwind.EntityModels;

namespace Northwind.WebApi;

static partial class Program
{
    internal static void MapCustomers(this WebApplication app)
    {
        app.MapGet(
            pattern: "/customers", 
            handler: async (ICustomerRepository repo,CancellationToken token = default) => await repo.RetrieveAllAsync(token));

        app.MapGet(
            pattern: "/customers/{id:regex(^[A-Z]{{5}}$)}",
            handler: async (string id, ICustomerRepository repo, CancellationToken token = default) => await repo.RetrieveAsync(id, token));

        app.MapGet(
            pattern: "/customers/in/{country}",
            handler: async (string? country, ICustomerRepository repo, CancellationToken token = default) => (await repo.RetrieveAllAsync(token)).Where(c => c.Country == country));


        app.MapPost(
            pattern: "/customers",
            handler: async Task<IResult> (Customer customer, ICustomerRepository repo, CancellationToken token = default) =>
            {
                if (customer is null) return Results.BadRequest("no customer to create");
                Customer? createdCustomer = await repo.CreateAsync(customer, token);
                if (createdCustomer is null) return Results.BadRequest("Failed to create a customer");
                return Results.Created(uri: "/customers", value: createdCustomer);
            });

        app.MapPut(
            pattern: "/customers/{id}",
            handler: async Task<IResult> (Customer customer, string id, ICustomerRepository repo, CancellationToken token = default) =>
            {
                if (customer is null || customer.CustomerId != id) return Results.BadRequest("no customer to update");
                Customer? updatedCustomer = await repo.UpdateAsync(customer, token);
                if (updatedCustomer is null) return Results.BadRequest("Failed to update a customer");
                return Results.Ok(value: updatedCustomer);
            });

        app.MapDelete(
            pattern: "/customers/{id}",
            handler: async Task<IResult> (string id, ICustomerRepository repo, CancellationToken token = default) =>
            {
                if (id is null) return Results.BadRequest("no customer to remove");
                bool deletedCustomer = await repo.DeleteAsync(id, token);
                if (!deletedCustomer) return Results.BadRequest("Failed to delete a customer");
                return Results.Ok(value: deletedCustomer);
            });
    }
}
