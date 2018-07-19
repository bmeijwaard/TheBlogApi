using System;
using System.Threading.Tasks;
using TheBlogApi.Data.Messages;
using TheBlogApi.Models.DTO;

namespace TheBlogApi.Data.Services.Contracts
{
    public interface IBaseService<TDto> where TDto : BaseDTO
    {
        Task<EntityResponse<TDto>> CreateAsync(TDto dto);
        Task<EntityResponse<TDto>> ReadAsync(Guid id);
        Task<EntityResponse<TDto>> UpdateAsync(TDto dto);
        Task<ServiceResponse> DeleteAsync(Guid id);
    }
}