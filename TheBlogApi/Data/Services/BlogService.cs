using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Data.Context.Providers.Contracts;
using TheBlogApi.Data.Messages;
using TheBlogApi.Data.Providers.Contracts;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Data.Stores;
using TheBlogApi.Models.Domain;
using TheBlogApi.Models.DTO;

namespace TheBlogApi.Data.Services
{
    public class BlogService : BaseService<Blog, BlogDTO>, IBlogService
    {
        private readonly IStorageProvider _storageProvider;

        public BlogService(IDbContextProvider contextProvider, IStorageProvider storageProvider) : base(contextProvider)
        {
            _storageProvider = storageProvider;
        }

        public override async Task<EntityResponse<BlogDTO>> ReadAsync(Guid id)
        {
            // because we always want to have to photo belonging to the blog we need to override the default read functionality.
            // there are now two ways to achieve this.

            var query = Query
                .Where(e => e.Id == id)
                .Include(a => a.BlogPhotos)
                .ThenInclude(bp => bp.Photo);

            var blogDTO = await MapFirstOrDefaultAsync(query).ConfigureAwait(false);

            return new EntityResponse<BlogDTO>(blogDTO);

            // return await ReadAsync(id, x => x.Include(a => a.BlogPhotos).ThenInclude(bp => bp.Photo));
        }


        public override async Task<EntityResponse<BlogDTO>> CreateAsync(BlogDTO dto)
        {
            return (EntityResponse<BlogDTO>)await TransactionAsync(async context =>
            {
                ICollection<BlogPhoto> blogPhotos = new List<BlogPhoto>();
                ICollection<Photo> photos = new List<Photo>();
                var blog = Mapper.Map<Blog>(dto);
                try
                {
                    // add the new blog to the context.
                    await context.Blogs.AddAsync(blog).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);

                    // process each photo linked to the new blog.
                    foreach (var photoDTO in dto.Photos)
                    {
                        // check if the photo already exists.
                        var photo = await context.Photos.FindAsync(photoDTO.Id).ConfigureAwait(false);

                        // else if a new photo is used, process and add a new photo to the context.
                        if (photo == null && photoDTO.NewPhoto != null)
                        {
                            photo = Mapper.Map<Photo>(photoDTO);
                            photo.ImageUrl = (await _storageProvider.SaveFileAndGetUriAsync(ContainerStore.PHOTO_CONTAINER, photoDTO.NewPhoto.FileName, photoDTO.NewPhoto).ConfigureAwait(false)).ToString();
                            photo.FileName = photoDTO.NewPhoto.FileName;
                            photos.Add(photo);
                            await context.Photos.AddAsync(photo).ConfigureAwait(false);
                            await context.SaveChangesAsync().ConfigureAwait(false);
                        }
                        if (photo == null) continue;

                        // stack the new many-to-many link until all photos are processed.
                        blogPhotos.Add(new BlogPhoto
                        {
                            BlogId = blog.Id,
                            PhotoId = photo.Id
                        });
                    }

                    // add all linked many-to-many relations between blog and photos.
                    await context.Set<BlogPhoto>().AddRangeAsync(blogPhotos).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);

                    // return the resulting blog entity.
                    return new EntityResponse<BlogDTO>(Mapper.Map<BlogDTO>(blog));
                }
                catch
                {
                    // if something in the process goes wrong, we undo the entire process.
                    // first we remove all succesful photo uploads.
                    foreach (var photo in photos)
                    {
                        await _storageProvider.DeleteAsync(ContainerStore.PHOTO_CONTAINER, photo.FileName).ConfigureAwait(false);
                    }

                    // then we re-throw the exception in order to let the context transaction rollback all database mutations.
                    throw;
                }
            }).ConfigureAwait(false);
        }
    }
}
