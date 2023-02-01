using BusReservationSystemApi.Data.Configuration;
using BusReservationSystemApi.Data.DTO.Request;
using BusReservationSystemApi.Data.DTO.Response;
using BusReservationSystemApi.Services.AdminServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BusReservationSystemApi.Controllers
{
    [Route("api/admin")]
    [Produces("application/json")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger _logger;
        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpGet("users")]
        public ActionResult<ServiceResponse<List<UserDataResponse>>> ListUsers(string UserName = "")
        {
            var response = _adminService.ListUsers(UserName);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpGet("roles")]
        public ActionResult<ServiceResponse<List<RoleDataResponse>>> ListRoles()
        {

            var response = _adminService.ListRoles();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("create-role"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> CreateRole(CreateRoleRequest request)
        {
            var response = await _adminService.CreateRole(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPut("change-role"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangeRole(RoleChangeRequest roleRequest)
        {
            var response = await _adminService.ChangeUserRole(roleRequest);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPut("change-userstatus"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangeUserStatus(ViewUserRequest viewUserRequest)
        {
            var response = await _adminService.ChangeUserStatus(viewUserRequest);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpPut("edit-user"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> EditUser(UserEditRequest userEditRequest)
        {
            var response = await _adminService.EditUser(userEditRequest);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpPut("edit-loggedIn-user"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> EditLoggedInUser(EditLoggedInUserRequest request)
        {
            var response = await _adminService.EditLoggedInUser(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpDelete("user")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteUser(string userId)
        {
            var response = await _adminService.DeleteUser(userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ServiceResponse<UserDataResponse>>> GetById(string userId)
        {
           
                var response = await _adminService.GetUserByIdAsync(userId);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            

        }

        [Authorize]
        [HttpGet("get-loggedin-user")]
        public async Task<ActionResult<ServiceResponse<UserDataResponse>>> GetLoggedInUser()
        {
            var response = await _adminService.GetLoggedInUser();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpGet("get-user-role")]
        public ActionResult<ServiceResponse<UserRoleResponse>> GetUserRole(string UserId)
        {
            var response = _adminService.ListUserRoles(UserId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
    }

}
