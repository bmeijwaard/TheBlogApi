using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TheBlogApi.Data.Context.Providers.Contracts;
using TheBlogApi.Data.Messages;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Data.Stores;
using TheBlogApi.Models.Domain;
using TheBlogApi.Models.DTO;

namespace TheBlogApi.Data.Services
{
    public class PhotoService : BaseService<Photo, PhotoDTO>, IPhotoService
    {
        private readonly IStorageProvider _storageProvider;

        public PhotoService(IDbContextProvider contextProvider, IStorageProvider storageProvider) : base(contextProvider)
        {
            _storageProvider = storageProvider;
        }

        public override async Task<EntityResponse<PhotoDTO>> CreateAsync(PhotoDTO dto)
        {
            return (EntityResponse<PhotoDTO>)await TransactionAsync(async context =>
            {
                var photo = Mapper.Map<Photo>(dto);
                try
                {
                    photo.ImageUrl = (await _storageProvider.SaveFileAndGetUriAsync(ContainerStore.PHOTO_CONTAINER, dto.NewPhoto.FileName, dto.NewPhoto).ConfigureAwait(false)).ToString();
                    photo.FileName = dto.NewPhoto.FileName;

                    await context.Photos.AddAsync(photo).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);

                    return new EntityResponse<PhotoDTO>(Mapper.Map<PhotoDTO>(photo));
                }
                catch
                {
                    await _storageProvider.DeleteAsync(ContainerStore.PHOTO_CONTAINER, photo.FileName).ConfigureAwait(false);
                    throw;
                }
            }).ConfigureAwait(false);
        }

        public async Task<EntityResponse<PhotoDTO>> CreateAsync(PhotoDTO dto, Guid blogId)
        {
            var blog = await DbSet<Blog>().FindAsync(blogId).ConfigureAwait(false);
            if (blog == null)
                return new EntityResponse<PhotoDTO>($"The entity {nameof(Blog).ToLower()} could not be found with id: {blogId}");            

            return (EntityResponse<PhotoDTO>)await TransactionAsync(async context =>
            {
                var photo = Mapper.Map<Photo>(dto);
                try
                {
                    photo.ImageUrl = (await _storageProvider.SaveFileAndGetUriAsync(ContainerStore.PHOTO_CONTAINER, dto.NewPhoto.FileName, dto.NewPhoto).ConfigureAwait(false)).ToString();
                    photo.FileName = dto.NewPhoto.FileName;

                    await context.Photos.AddAsync(photo).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);

                    var blogPhoto = new BlogPhoto
                    {
                        BlogId = blog.Id,
                        PhotoId = photo.Id
                    };

                    await context.Set<BlogPhoto>().AddAsync(blogPhoto).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);

                    return new EntityResponse<PhotoDTO>(Mapper.Map<PhotoDTO>(photo));
                }
                catch
                {
                    await _storageProvider.DeleteAsync(ContainerStore.PHOTO_CONTAINER, photo.FileName).ConfigureAwait(false);
                    throw;
                }
            }).ConfigureAwait(false);
        }

        public override async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            var photo = await Query.FindAsync(id).ConfigureAwait(false);
            if (photo == null)
                return new ServiceResponse($"The photo could not be found with id: {id}");

            return (ServiceResponse)await TransactionAsync(async context =>
            {
                var blogPhoto = await context.Set<BlogPhoto>().FirstOrDefaultAsync(bp => bp.PhotoId == id);
                if (blogPhoto != null)
                    context.Set<BlogPhoto>().Remove(blogPhoto);

                context.Photos.Remove(photo);
                await context.SaveChangesAsync().ConfigureAwait(false);

                await _storageProvider.DeleteAsync(ContainerStore.PHOTO_CONTAINER, photo.FileName);

                return new ServiceResponse();

            }).ConfigureAwait(false);
        }
    }
}
