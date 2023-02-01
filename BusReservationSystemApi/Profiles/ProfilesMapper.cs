using AutoMapper;
using BusReservationSystemApi.Data.DTO.Request;
using BusReservationSystemApi.Data.DTO.Response;
using BusReservationSystemApi.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BusReservationSystemApi.Profiles
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {

            //request mapping
            CreateMap<UserRegisterRequest, AppUser>();

            CreateMap<UserEditRequest, AppUser>();
            //response mapping
            CreateMap<AppUser, UserDataResponse>();
            //CreateMap<AppUser, ViewUserResponse>();
            CreateMap<IdentityRole, RoleDataResponse>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Name));

        }
    }
}
