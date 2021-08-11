using AutoMapper;
using Exist.ViewModels;

namespace Exist.Models.Mapper
{
    public class MappingEntity : Profile
    {
        public MappingEntity()
        {
            CreateMap<User, UserView>().ReverseMap();
            CreateMap<User, UserView>().ReverseMap();
        }
    }
}
