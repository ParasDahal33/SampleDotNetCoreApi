
using BusReservationSystemApi.Data.Configuration;
using BusReservationSystemApi.Data.DTO.Request;
using BusReservationSystemApi.Data.DTO.Response;

namespace BusReservationSystemApi.Services.AdminServices
{
    public interface IAdminService
    {
        ServiceResponse<List<RoleDataResponse>> ListRoles();
        ServiceResponse<UserRoleResponse> ListUserRoles(string userId);
        ServiceResponse<List<UserDataResponse>> ListUsersAsync(string search);
        Task<ServiceResponse<bool>> ChangeUserRole(RoleChangeRequest roleRequest);
        Task<ServiceResponse<bool>> CreateRole(CreateRoleRequest request);
        Task<ServiceResponse<bool>> ChangeUserStatus(ViewUserRequest viewUserRequest);
        Task<ServiceResponse<bool>> EditUser(UserEditRequest userEditRequest);
        Task<ServiceResponse<bool>> EditLoggedInUser(EditLoggedInUserRequest request);
        Task<ServiceResponse<bool>> DeleteUser(string userId);
        Task<ServiceResponse<UserDataResponse>> GetUserByIdAsync(string userId);
        Task<ServiceResponse<UserDataResponse>> GetLoggedInUser();


    }
}
