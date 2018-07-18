using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Data.Context.Providers.Contracts;
using TheBlogApi.Data.Messages;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Models.Domain;
using TheBlogApi.Models.DTO;

namespace TheBlogApi.Data.Services
{
    public class BlogService : BaseService<Blog, BlogDTO>, IBlogService //2EF4B0D9-4789-E811-80C2-000D3A228E10
    {
        private readonly IDbContextProvider _contextProvider;

        public BlogService(IDbContextProvider contextProvider) : base(contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public async Task<EntityResponse<BlogDTO>> ReadAsync(Guid id)
        {
            return await ReadAsync(id, x => x.Include(a => a.BlogPhotos).ThenInclude(bp => bp.Photo));
        }

        public override async Task<EntityResponse<BlogDTO>> CreateAsync(BlogDTO dto)
        {
            if (await _contextProvider.Context.Blogs.AnyAsync(e => e.Id == dto.Id).ConfigureAwait(false))
                return new EntityResponse<BlogDTO>($"The entity {nameof(Blog).ToLower()} already exists with id: {dto.Id}");

            var entity = Mapper.Map<Blog>(dto);
            entity.UserId = new Guid("2EF4B0D9-4789-E811-80C2-000D3A228E10");

            return (EntityResponse<BlogDTO>)await _contextProvider.ExecuteTransactionAsync(async context =>
            {
                await context.Set<Blog>().AddAsync(entity).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                return new EntityResponse<BlogDTO>(Mapper.Map<BlogDTO>(entity));

            }).ConfigureAwait(false);
        }
    }
}
