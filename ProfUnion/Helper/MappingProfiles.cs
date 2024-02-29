using AutoMapper;
using Profunion.Dto.UserDto;
using Profunion.Models;

namespace Profunion.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            
            CreateMap<User, LoginUserDto>();
            CreateMap<LoginUserDto, User>();


        }
    }
}
