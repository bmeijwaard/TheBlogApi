using AutoMapper;
using TheBlogApi.Models.Domain;
using TheBlogApi.Models.DTO;
using TheBlogApi.Models.Requests;

namespace TheBlogApi.Config
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<RegisterRequest, UserDTO>();

            CreateMap<PasswordRequest, TokenRequest>();
            CreateMap<TenantRequest, TokenRequest>();

            CreateMap<PhotoRequest, PhotoDTO>();
            CreateMap<PhotoRequest, PhotoBaseDTO>();
        }
    }
}
