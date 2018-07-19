using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Data.Context.Contracts;
using TheBlogApi.Data.Context.Providers.Contracts;
using TheBlogApi.Data.Messages;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Models.Domain;
using TheBlogApi.Models.DTO;

namespace TheBlogApi.Data.Services
{
    public abstract class BaseService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : BaseDTO
    {
        private readonly IDbContextProvider _contextProvider;

        public BaseService(IDbContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public virtual async Task<EntityResponse<TDto>> CreateAsync(TDto dto)
        {
            if (await _contextProvider.Context.Set<TEntity>().AnyAsync(e => e.Id == dto.Id).ConfigureAwait(false))
                return new EntityResponse<TDto>($"The entity {nameof(TEntity).ToLower()} already exists with id: {dto.Id}");

            var entity = Mapper.Map<TEntity>(dto);

            return (EntityResponse<TDto>)await _contextProvider.ExecuteTransactionAsync(async context =>
            {
                await context.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                return new EntityResponse<TDto>(Mapper.Map<TDto>(entity));

            }).ConfigureAwait(false);
        }

        public virtual async Task<EntityResponse<TDto>> ReadAsync(Guid id)
        {
            return await ReadAsync(id, null).ConfigureAwait(false);
        }

        protected virtual async Task<EntityResponse<TDto>> ReadAsync(Guid id, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            IQueryable<TEntity> query = _contextProvider.Context.Set<TEntity>();

            if (include != null) query = include(query);

            var entity = await query.FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
            if (entity == null)
                return new EntityResponse<TDto>($"The entity {nameof(TEntity).ToLower()} could not be found with id: {id}");

            return new EntityResponse<TDto>(Mapper.Map<TDto>(entity));
        }

        public virtual async Task<EntityResponse<TDto>> UpdateAsync(TDto dto)
        {
            var entity = await _contextProvider.Context.Set<TEntity>().FindAsync(dto.Id).ConfigureAwait(false);
            if (entity == null)
                return new EntityResponse<TDto>($"The entity {nameof(TEntity).ToLower()} could not be found with id: {dto.Id}");

            Mapper.Map(dto, entity);
            return (EntityResponse<TDto>)await _contextProvider.ExecuteTransactionAsync(async context =>
            {
                context.Set<TEntity>().Update(entity);
                await context.SaveChangesAsync().ConfigureAwait(false);

                return new EntityResponse<TDto>(Mapper.Map<TDto>(entity));

            }).ConfigureAwait(false);
        }

        public virtual async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            var entity = await _contextProvider.Context.Set<TEntity>().FindAsync(id);
            if (entity == null)
                return new EntityResponse<TDto>($"The entity {nameof(TEntity).ToLower()} could not be found with id: {id}");

            return (ServiceResponse)await _contextProvider.ExecuteTransactionAsync(async context =>
            {
                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync().ConfigureAwait(false);

                return new ServiceResponse();

            }).ConfigureAwait(false);
        }

        protected virtual DbSet<TEntity> Query
        {
            get
            {
                return _contextProvider.Context.Set<TEntity>();
            }
        }

        protected virtual DbSet<T> DbSet<T>() where T : BaseEntity
        {
            return _contextProvider.Context.Set<T>();
        }

        protected virtual async Task<IServiceResponse> TransactionAsync<T>(Func<IDbContext, Task<T>> contextFunc) where T : IServiceResponse
        {
            return await _contextProvider.ExecuteTransactionAsync(contextFunc).ConfigureAwait(false);
        }

        protected virtual async Task<IEnumerable<TDto>> MapToListAsync(IQueryable<TEntity> query)
        {
            return await query.Select(e => Mapper.Map<TDto>(e)).ToListAsync().ConfigureAwait(false);
        }

        protected virtual async Task<TDto> MapFirstOrDefaultAsync(IQueryable<TEntity> query)
        {
            return Mapper.Map<TDto>(await query.FirstOrDefaultAsync().ConfigureAwait(false));
        }
    }
}
