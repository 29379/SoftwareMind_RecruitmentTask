using AutoMapper;
using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Dto.Booking;
using HotDeskBookingSystem.Data.Dto.Desk;
using HotDeskBookingSystem.Data.Dto.User;
using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Data
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<BookingInfoDto, Booking>();
            CreateMap<CreateBookingDto, Booking>();

            CreateMap<DeskCreationDto, Desk>();
            CreateMap<DeskDto, Desk>();

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

            CreateMap<OfficeDto, Office>();
            CreateMap<OfficeFloorDto, OfficeFloor>();
        }
    }
}
