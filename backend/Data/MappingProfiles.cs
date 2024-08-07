using AutoMapper;
using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Data
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<LoginDto, AppUser>();
            CreateMap<AppUser, AppUserDto>()
                .ForMember(
                dest => dest.Roles, 
                opt => opt.MapFrom(
                    src => src.UserRoles.Select(
                        ur => ur.RoleName).ToList()
                        )
                );
            CreateMap<UpdateUserDto, AppUser>();
        }
    }
}
