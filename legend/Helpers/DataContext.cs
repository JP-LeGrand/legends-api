namespace legend.Helpers;

using Microsoft.EntityFrameworkCore;
using legend.Entities;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sql server database
        options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User to Address relationship
        modelBuilder.Entity<User>()
            .HasMany(u => u.DeliveryAddresses)
            .WithOne()
            .HasForeignKey(a => a.UserId);

        base.OnModelCreating(modelBuilder);
    }
}