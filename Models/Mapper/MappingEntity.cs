using AutoMapper;
using Exist.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
