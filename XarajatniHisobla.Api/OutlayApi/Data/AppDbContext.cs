using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OutlayApi.Entities;
using System.Reflection;

namespace OutlayApi.Data;
public class AppDbContext : IdentityDbContext<User, UserRole, int>
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Outlay> Outlays { get; set; }
    public DbSet<Debt> Debts { get; set; }  
    public DbSet<UserCategory> UserCategories { get; set; } 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
