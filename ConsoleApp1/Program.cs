// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;

AppDbContext context = new();

List<ProductOrderDTO> datas =await context.Products.Include(x => x.Orders).Select(p => new ProductOrderDTO()
{
    ProductId = p.ProductId,
    ProductName=p.ProductName,
    Orders=p.Orders
}).ToListAsync();


List<ProductOrderDTO2> datas2 = await context.Products.Include(x => x.Orders).Select(x => new ProductOrderDTO2() {
    ProductName = x.ProductName,
    Description= x.Orders.AsQueryable().Select(p=>p.Description).ToList()
}).ToListAsync();


Console.WriteLine("");


public class Product
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public ICollection<Order>? Orders { get; set; }
}
public class Order
{
    public int OrderId { get; set; }
    public string? Description { get; set; }
    public int ProductID { get; set; }
    public Product? Product { get; set; }

}
public class ProductOrderDTO:Product
{
    public ICollection<Order>? Orders { get; set; }
}
public class ProductOrderDTO2
{
    public string? ProductName { get; set; }
    public List<string>? Description { get; set; }
}
public class AppDbContext:DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasKey(p=> p.ProductId);
        modelBuilder.Entity<Product>().HasMany(p=>p.Orders).WithOne(o=>o.Product).HasForeignKey(f=>f.ProductID);
        modelBuilder.Entity<Order>().HasKey(o=>o.OrderId);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=OLGUNPC\\SQLEXPRESS; Initial Catalog=DTOS; Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
    }
}