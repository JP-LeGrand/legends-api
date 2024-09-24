namespace legend.Helpers;

using Microsoft.EntityFrameworkCore;

public class SqliteDataContext : DataContext
{
    public SqliteDataContext(IConfiguration configuration) : base(configuration) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlite database
        options.UseSqlite($"Data Source={Path.Combine(AppContext.BaseDirectory, Configuration.GetConnectionString("WebApiDatabase"))}");
    }
}