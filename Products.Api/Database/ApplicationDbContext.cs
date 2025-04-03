using Microsoft.EntityFrameworkCore;
using Products.Api.Entities;

namespace Products.Api.Database;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    //public DbSet<ProductOlder> Product { get; set; }
    public DbSet<Products.Api.Entities.Products> Products { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }

}