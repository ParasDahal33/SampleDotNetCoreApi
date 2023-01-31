using AutoMapper;
using BusReservationSystemApi.Data.Configuration;
using BusReservationSystemApi.Data.DBContext;
using BusReservationSystemApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace BusReservationSystemApi.Services.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,
            ApplicationDbContext db, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

    }

}
