using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace Core.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig() {
            CreateMap<Users, UsersDto>().ReverseMap();
            CreateMap<TaskUser, TaskUserDto>().ReverseMap();
        }
    }
}
