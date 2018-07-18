using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogApi.Helpers.Extensions;
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

            CreateMap<Blog, BlogDTO>()
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.BlogPhotos.Select(bp => bp.Photo)))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.ToStringArray()));
            CreateMap<BlogDTO, Blog>()
                .ForMember(dest => dest.BlogPhotos, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.ToArrayString()));

            CreateMap<Blog, BlogBaseDTO>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.ToStringArray()));
            CreateMap<BlogBaseDTO, Blog>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.ToArrayString()));

            CreateMap<Photo, PhotoDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => new Uri(src.ImageUrl)))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => new Uri(src.ThumbnailUrl)))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.ToStringArray()));
            CreateMap<PhotoDTO, Photo>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl.ToString()))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailUrl.ToString()))
                .ForMember(dest => dest.BlogPhotos, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.ToArrayString()));

            CreateMap<Photo, PhotoBaseDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => new Uri(src.ImageUrl)))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => new Uri(src.ThumbnailUrl)));
            CreateMap<PhotoBaseDTO, Photo>()
                 .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl.ToString()))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailUrl.ToString()));
        }
    }
}
