using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheBlogApi.Models.Domain;

namespace TheBlogApi.Data.Context.Contracts
{
    public interface IDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Tenant> Tenants { get; }
        DbSet<Blog> Blogs { get; }
        DbSet<Photo> Photos { get; }

        void Dispose();
        DbConnection GetConnection();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<IDbContextTransaction> BeginTransactionAsync();
        IDbContextTransaction BeginTransaction();
        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
        EntityEntry Entry(object entity);
        EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> ExecuteSqlCommandAsync(RawSqlString stp, params object[] parameters);
        Task BulkInsertAsync<T>(IList<T> entities) where T : class;
    }
}
