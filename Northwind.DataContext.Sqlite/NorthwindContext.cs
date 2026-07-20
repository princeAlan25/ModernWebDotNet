using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Northwind.EntityModels;


namespace Northwind.DataContext.Sqlite;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext()
    {
    }

    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            string database = "Northwind.db";
            string currentDir = CurrentDirectory;
            string databasePath = string.Empty;

            if (currentDir.EndsWith("net9.0") || currentDir.EndsWith("net10.0"))
            {
                // In the <project>\bin\<Debug|Release>\net9.0 directory.
                databasePath = Combine("..", "..", "..", "..", database);
            }
            else
            {
                // In the <project> directory.
                databasePath = Combine("..", database);
            }

            databasePath = GetFullPath(databasePath);

            try
            {
                NorthwindContextLogger.WriteLine($"Database Path: {databasePath}");
            }
            catch(Exception ex)
            {
                NorthwindContextLogger.WriteLine(ex.Message);
            }

            if (!File.Exists(databasePath)) throw new FileNotFoundException($"Datbase Path/file {databasePath} not found", fileName: databasePath);

            optionsBuilder.UseSqlite($"Data Source={databasePath}");

            optionsBuilder.LogTo(
                NorthwindContextLogger.WriteLine,
                [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]
                );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(c => c.Customer).WithMany(c => c.Orders).HasForeignKey(c => c.CustomerId).OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.Freight).HasDefaultValue(0.0M);

        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.Quantity).HasDefaultValue((short)1);
            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
            entity.Property(e => e.UnitPrice).HasDefaultValue(0.0M);
            entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
            entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
