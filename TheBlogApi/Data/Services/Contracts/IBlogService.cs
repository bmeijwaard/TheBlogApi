using System;
using System.Threading.Tasks;
using TheBlogApi.Data.Messages;
using TheBlogApi.Data.Services.Contracts;
using TheBlogApi.Models.DTO;

namespace TheBlogApi.Data.Services.Contracts
{
    public interface IBlogService : IBaseService<BlogDTO>
    {
        Task<EntityResponse<BlogDTO>> ReadAsync(Guid id);
    }
}