using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheBlogApi.Data.Context.Contracts;
using TheBlogApi.Data.Context.ModelBuilders;
using TheBlogApi.Models.Domain;

namespace TheBlogApi.Data.Context
{
    public class SqlContext : IdentityDbContext<User, Role, Guid>, IDbContext
    {
        public SqlContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //override for aspnetuser tables
            builder.CreateSecurityModel();

            //set database generated properties
            builder.Entity<User>(c => c.Property(i => i.RegisteredDateUTC).HasDefaultValueSql("GETUTCDATE()"));
            builder.Entity<Photo>(c => c.Property(i => i.PublicationDateUtc).HasDefaultValueSql("GETUTCDATE()"));
            builder.Entity<Blog>(c => c.Property(i => i.PublicationDateUtc).HasDefaultValueSql("GETUTCDATE()"));

            // many-to-many coupling
            builder.Entity<BlogPhoto>().HasKey(t => new { t.BlogId, t.PhotoId });

            builder.Entity<BlogPhoto>()
                .HasOne(bp => bp.Blog)
                .WithMany(b => b.BlogPhotos)
                .HasForeignKey(bp => bp.BlogId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BlogPhoto>()
                .HasOne(bp => bp.Photo)
                .WithMany(p => p.BlogPhotos)
                .HasForeignKey(bp => bp.PhotoId)
                .OnDelete(DeleteBehavior.Restrict);
        }


            public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return Database.BeginTransaction(isolationLevel);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await Database.BeginTransactionAsync();
        }

        public DbConnection GetConnection()
        {
            return Database.GetDbConnection();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<int> ExecuteSqlCommandAsync(RawSqlString stp, params object[] parameters)
        {
            return await Database.ExecuteSqlCommandAsync(stp, parameters);
        }

        public async Task BulkInsertAsync<T>(IList<T> entities) where T : class
        {
            await DbContextBulkExtensions.BulkInsertAsync(this, entities);
        }
    }
}
