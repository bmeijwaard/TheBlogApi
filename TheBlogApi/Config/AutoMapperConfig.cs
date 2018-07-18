using AutoMapper;

namespace TheBlogApi.Config
{
    public class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ServiceProfile>();
                cfg.AddProfile<ApiProfile>();
            });
        }
    }
}
