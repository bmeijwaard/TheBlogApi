using System;
using System.Threading.Tasks;
using TheBlogApi.Data.Messages;
using TheBlogApi.Models.DTO;

namespace TheBlogApi.Data.Services.Contracts
{
    public interface IPhotoService : IBaseService<PhotoDTO>
    {
    }
}