using BusReservationSystemApi.Data.Configuration;
using BusReservationSystemApi.Data.DTO.Request;
using BusReservationSystemApi.Data.DTO.Response;
using BusReservationSystemApi.Data.Enumeration;
using BusReservationSystemApi.Data.Models;
using BusReservationSystemApi.Filters;
using BusReservationSystemApi.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BusReservationSystemApi.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private ILogger _logger;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(IAuthService authService, UserManager<AppUser> userManager, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _authService = authService;
            _logger = logger;
        }


        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<ServiceResponse<bool>>> Register(UserRegisterRequest registerRequest)
        {
            var response = await _authService.RegisterUser(registerRequest);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            _logger.LogInformation("User Successfully Registered");

            return Ok(response);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<ServiceResponse<UserLoginResponse>>> Login(UserLoginRequest userLoginRequest)
        {
            var user = await _userManager.FindByEmailAsync(userLoginRequest.Email);
            if (user != null)
            {
                var response = await _authService.ValidateUser(userLoginRequest);
                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }
                string ip = Response.HttpContext.Connection.RemoteIpAddress.ToString();
                if (ip == "::1")
                {
                    ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.LastOrDefault()?.ToString() ?? "IP NOT FOUND";
                }

                _logger.LogInformation($"User with email {userLoginRequest.Email} Successfully Logged in at time {DateTime.Now} with Ip {ip}");
                return Ok(response);

            }
            return NotFound($"User with email {userLoginRequest.Email} not found");
        }

        [HttpGet("reset-password")]
        public ActionResult<ServiceResponse<bool>> ResetPassword(string id = null, string token = null)
        {

            UserPasswordResetRequest userPasswordResetRequest = new UserPasswordResetRequest();
            {
                userPasswordResetRequest.Id = id;
                userPasswordResetRequest.Token = token;
            }
            return token == null ? BadRequest() : Ok(userPasswordResetRequest);
        }

        [HttpPost("reset-password")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<ServiceResponse<bool>>> ResetPassword(UserPasswordResetRequest userPasswordResetRequest)
        {
            var response = await _authService.ResetPassword(userPasswordResetRequest);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("change-password")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword(ChangePasswordRequest request)
        {
            var response = await _authService.ChangePassword(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("extend-password")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<ServiceResponse<bool>>> extendPassword(string email, ExtendPasswordExpiryRequest extendPasswordExpiryRequest)
        {
            var response = await _authService.ExtendPassword(email, extendPasswordExpiryRequest);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ServiceResponse<bool>>> ForgotPassword(string email, string userType)
        {
            var response = await _authService.ForgotPassword(email, userType);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult<ServiceResponse<bool>>> ConfirmEmail(string id, string token)
        {
            var response = await _authService.ConfirmEmail(id, token);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult<ServiceResponse<bool>>> VerifyEmail([FromBody] string email)
        {
            var response = await _authService.SendEmailConfirmation(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }



        [HttpPost("refresh-token")]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<ActionResult<ServiceResponse<UserLoginResponse>>> RefreshToken(TokenRefreshRequest tokenRefreshRequest)
        {
            var response = await _authService.RefreshToken(tokenRefreshRequest);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("revoke-usertoken/{userId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RevokeUser(string userId)
        {
            var response = await _authService.RevokeToken(userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Route("revoke-loggedIn-user")]
        public async Task<ActionResult<ServiceResponse<bool>>> RevokeLogeedInuser()
        {
            var response = await _authService.RevokeLoggedInUser();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("revoke-all-token")]
        public async Task<ActionResult<ServiceResponse<bool>>> RevokeAllUser()
        {
            var response = await _authService.RevokeAllToken();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("verify")]
        [Authorize]
        public IActionResult Verify()
        {
            // TODO Add your client auth here.
            return Ok("User Verified!");
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("resend-email-verification")]
        public async Task<ActionResult<ServiceResponse<bool>>> ReSendEmailConfirmation(ViewUserRequest request)
        {
            var response = await _authService.ReSendEmailConfirmation(request.UserId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
    }
}
