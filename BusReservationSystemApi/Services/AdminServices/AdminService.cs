using AutoMapper;
using BusReservationSystemApi.Data.Configuration;
using BusReservationSystemApi.Data.DBContext;
using BusReservationSystemApi.Data.DTO.Request;
using BusReservationSystemApi.Data.DTO.Response;
using BusReservationSystemApi.Data.Enumeration;
using BusReservationSystemApi.Data.Models;
using BusReservationSystemApi.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<ServiceResponse<bool>> ChangeUserRole(RoleChangeRequest roleRequest)
        {
            var user = await _userManager.FindByIdAsync(roleRequest.UserId);
            var oldRole = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, oldRole);
            if (!result.Succeeded)
                return ServiceResponse<bool>.Failed("cannot remove existing role", null);
            var role = await _roleManager.FindByNameAsync(roleRequest.Role);
            if (role == null)
                return ServiceResponse<bool>.Failed("Cannot change to selected role", null);
            result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
                return ServiceResponse<bool>.Failed("Cannot change to selected role", null);
            return ServiceResponse<bool>.Succeeded(true, "Role changed successfully");
        }

        public async Task<ServiceResponse<bool>> ChangeUserStatus(ViewUserRequest viewUserRequest)
        {
            var user = await _userManager.FindByIdAsync(viewUserRequest.UserId);
            if (user == null)
            {
                return ServiceResponse<bool>.Failed("User cannot be found", null);
            }
            if (user.UserStatus != UserStatus.Active)
            {
                user.UserStatus = UserStatus.Active;
                _db.AppUser.Update(user);
                _db.SaveChanges();
                return ServiceResponse<bool>.Succeeded(true, "User has been Activated");
            }
            user.UserStatus = UserStatus.Inactive;
            _db.AppUser.Update(user);
            _db.SaveChanges();
            return ServiceResponse<bool>.Succeeded(true, "User has been Deactivated");
        }

        public async Task<ServiceResponse<bool>> CreateRole(CreateRoleRequest request)
        {
            var rolecheck = _db.Roles.FirstOrDefault(r => r.Name == request.Name);
            if (rolecheck != null)
            {
                return ServiceResponse<bool>.Failed($"User role with name {request.Name} already exists .", null);
            }
            await _roleManager.CreateAsync(new IdentityRole() { Name = request.Name });
            await _db.SaveChangesAsync();
            return ServiceResponse<bool>.Succeeded(true, "Role Created Successfully.");
        }

        public async Task<ServiceResponse<bool>> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResponse<bool>.Failed("User cannot be found", null);
            }
            _db.AppUser.Remove(user);
            await _db.SaveChangesAsync();
            return ServiceResponse<bool>.Succeeded(true, "User deleted Successfully");
        }

        public async Task<ServiceResponse<bool>> EditLoggedInUser(EditLoggedInUserRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResponse<bool>.Failed("User not found.", null);
            }
            var oldUserName = user.UserName;
            var oldFullName = user.FullName;
            if (request.UserName == "")
            {
                user.UserName = oldUserName;
                user.FullName = request.FullName;
            }
            if (request.FullName == "")
            {
                user.UserName = request.UserName;
                user.FullName = oldFullName;
            }
            if (request.UserName != "" && request.FullName != "")
            {
                user.UserName = request.UserName;
                user.FullName = request.FullName;
            }
            _db.AppUser.Update(user);
            await _db.SaveChangesAsync();
            return ServiceResponse<bool>.Succeeded(true, "Successful.");
        }

        public async Task<ServiceResponse<bool>> EditUser(UserEditRequest userEditRequest)
        {
            var user = await _userManager.FindByIdAsync(userEditRequest.Id);
            if (user == null)
            {
                return ServiceResponse<bool>.Failed("User cannot be found", null);
            }
            user.FullName = userEditRequest.FullName;
            user.UserName = userEditRequest.UserName;
            user.Email = userEditRequest.Email;
            user.UserStatus = userEditRequest.UserStatus;
            await _userManager.UpdateAsync(user);
            var oldRole = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, oldRole);
            if (!result.Succeeded)
                return ServiceResponse<bool>.Failed("cannot remove existing role", null);
            var role = await _roleManager.FindByNameAsync(userEditRequest.Role);
            if (role == null)
                return ServiceResponse<bool>.Failed("Cannot change to selected role", null);
            result = await _userManager.AddToRoleAsync(user, role.Name);
            await _db.SaveChangesAsync();
            return ServiceResponse<bool>.Succeeded(true, "User has been edited sucessfully.");
        }

        public async Task<ServiceResponse<UserDataResponse>> GetLoggedInUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var userRole = _db.UserRoles.FirstOrDefault(r => r.UserId == user.Id);
            var role = _db.Roles.FirstOrDefault(x => x.Id == userRole.RoleId);
            if (role != null)
            {
                user.Role = role.Name;
            }

            var mapUser = _mapper.Map<UserDataResponse>(user);
            return ServiceResponse<UserDataResponse>.Succeeded(mapUser, "Success");
        }

        public async Task<ServiceResponse<UserDataResponse>> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRole = _db.UserRoles.FirstOrDefault(r => r.UserId == userId);
            var role = _db.Roles.FirstOrDefault(x => x.Id == userRole.RoleId);
            if (role != null)
            {
                user.Role = role.Name;
            }

            var mapUser = _mapper.Map<UserDataResponse>(user);
            if (user == null)
            {
                return ServiceResponse<UserDataResponse>.Failed($"User with userId {userId} not found", null);
            }
            else
            {
                return ServiceResponse<UserDataResponse>.Succeeded(mapUser, "Success");
            }
        }

        public ServiceResponse<List<RoleDataResponse>> ListRoles()
        {
            var roles = _roleManager.Roles.ToList();
            var mapRoles = _mapper.Map<List<RoleDataResponse>>(roles);
            return ServiceResponse<List<RoleDataResponse>>.Succeeded(mapRoles, "List retrieved successfully");

        }

        public ServiceResponse<UserRoleResponse> ListUserRoles(string userId)
        {
            var user = _db.AppUser.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return ServiceResponse<UserRoleResponse>.Failed("User Not found", null);
            }
            var userRole = _db.UserRoles.FirstOrDefault(r => r.UserId == userId);
            if (userRole == null)
            {
                return ServiceResponse<UserRoleResponse>.Failed("User Role Not Declared.", null);
            }
            var role = _db.Roles.FirstOrDefault(x => x.Id == userRole.RoleId);
            if (role == null)
            {
                return ServiceResponse<UserRoleResponse>.Failed("User Role Not Found.", null);
            }
            return ServiceResponse<UserRoleResponse>.Succeeded(new UserRoleResponse
            { UserId = userId, RoleId = userRole.RoleId, RoleName = role.Name }, "Role retrived Successfully.");

        }

        public ServiceResponse<List<UserDataResponse>> ListUsersAsync(string search)
        {
            var searchParams = search ?? "";
            var listByFullName = _db.AppUser.Where(d => d.FullName.ToLower().Contains(searchParams.ToLower())).OrderByDescending(x => x.AccountCreatedDate);
            
            var mapUser = _mapper.Map<List<UserDataResponse>>(listByFullName);
            if (mapUser == null)
            {
                return ServiceResponse<List<UserDataResponse>>.Failed("No User Found.", null);
            }
            foreach (var user in mapUser)
            {
                var userRole = _db.UserRoles.FirstOrDefault(r => r.UserId == user.Id);
                if (userRole == null)
                {
                    return ServiceResponse<List<UserDataResponse>>.Failed("User Role Not Declared.", null);
                }
                var role = _db.Roles.FirstOrDefault(x => x.Id == userRole.RoleId);
                user.Role = role.Name;
            }
            return ServiceResponse<List<UserDataResponse>>.Succeeded(mapUser, "List Retrieved Successfully.");
        }
    }

}
