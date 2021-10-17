using AutoMapper;
using Web.Cars.Data.Identity;
using Web.Cars.Models;

namespace Web.Cars.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<RegisterViewModel, AppUser>()
                .ForMember(x => x.Photo, opt => opt.Ignore())
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
