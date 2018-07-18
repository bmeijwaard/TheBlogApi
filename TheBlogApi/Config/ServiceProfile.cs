using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Models.Domain;
using TheBlogApi.Models.DTO;

namespace TheBlogApi.Config
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<UserBaseDTO, User>();
            CreateMap<User, UserBaseDTO>();

            CreateMap<TenantDTO, Tenant>();
            CreateMap<Tenant, TenantDTO>();

            CreateMap<TenantBaseDTO, Tenant>();
            CreateMap<Tenant, TenantBaseDTO>();
        }
    }
}
