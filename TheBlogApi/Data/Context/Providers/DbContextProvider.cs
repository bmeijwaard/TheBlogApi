using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Config.Settings;
using TheBlogApi.Data.Context.Contracts;
using TheBlogApi.Data.Context.Providers.Contracts;
using TheBlogApi.Data.Handlers;
using TheBlogApi.Data.Messages;

namespace TheBlogApi.Data.Context.Providers
{
    public class DbContextProvider : IDbContextProvider
    {
        private readonly string _connectionString;
        private readonly IExceptionHandler _exceptionHandler;

        public DbContextProvider(Func<SqlContext> context, IOptions<ConnectionStrings> connectionStrings, IExceptionHandler exceptionHandler)
        {
            Context = context();
            _connectionString = connectionStrings.Value.DefaultConnection;
            _exceptionHandler = exceptionHandler;
        }

        public IDbContext Context { get; }

        public async Task<IServiceResponse> ExecuteTransactionAsync<T>(Func<IDbContext, Task<T>> contextFunc) where T : IServiceResponse
        {
            try
            {
                T result = default(T);
                using (var contextTransaction = await _context.BeginTransactionAsync())
                {
                    try
                    {
                        result = await contextFunc(_context);
                        contextTransaction.Commit();

                        return result;
                    }
                    catch (Exception e)
                    {
                        // we catch this exception to suppress (known) optimistic concurrency issue's
                        if (e.Message.Contains("Database operation expected to affect 1 row(s) but actually affected 0 row(s)."))
                        {
                            return result;
                        }
                        contextTransaction.Rollback();
                        await _exceptionHandler.ProcessExceptionAsync("An error occured within an entity mutation.", e, "ExecuteTransactionAsync", result);
                        throw e;
                    }
                }
            }
            finally
            {
                _context.Dispose();
            }
        }

        private IDbContext _context
        {
            get
            {
                return InitializeContext();
            }
        }

        private IDbContext InitializeContext()
        {
            var builder = new DbContextOptionsBuilder<SqlContext>();
            builder.UseSqlServer(_connectionString);
            return new SqlContext(builder.Options);
        }
    }
}
