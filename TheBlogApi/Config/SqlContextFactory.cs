using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TheBlogApi.Data.Context;

namespace TheBlogApi
{
    public class SqlContextFactory : IDesignTimeDbContextFactory<SqlContext>
    {
        public SqlContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var builder = new DbContextOptionsBuilder<SqlContext>();
            builder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            return new SqlContext(builder.Options);
        }
    }
}
