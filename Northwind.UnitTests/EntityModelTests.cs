using Northwind.DataContext.Sqlite;
using Northwind.EntityModels;

namespace Northwind.UnitTests;

public class EntityModelTests
{
    [Fact]
    public void DatabaseConnectTest()
    {
        using NorthwindContext db = new();
        Assert.True(db.Database.CanConnect());
    }

    [Fact]
    public void CategoryCountTest()
    {
        using NorthwindContext db = new();
        int expected = 8;
        int actualValue = db.Categories.Count();
        Assert.Equal(expected, actualValue);
    }

    [Fact]
    public void ProductId1IsChaiTest()
    {
        using NorthwindContext db = new();
        string expected = "Chai";
        Product? product = db.Products.Find(keyValues: 1);
        string actualIdValue = product?.ProductName ?? string.Empty;
        Assert.Equal(expected, actualIdValue);
    }
}
