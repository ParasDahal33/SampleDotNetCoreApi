
using AutoMapper;
using BusReservationSystemApi.Data.Configuration;
using BusReservationSystemApi.Data.DBContext;
using BusReservationSystemApi.Data.DTO.Request;
using BusReservationSystemApi.Data.DTO.Response;
using BusReservationSystemApi.Data.Enum;
using BusReservationSystemApi.Data.Models;
using BusReservationSystemApi.Helpers.Tokens;
using BusReservationSystemApi.Services.EmailSenderService;
using Microsoft.AspNetCore.Identity;

namespace BusReservationSystemApi.Services.AuthServices
{
    public partial class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmailConfirmTokenHelper _emailConfirmToken;
        private readonly IPasswordResetTokenHelper _passwordResetToken;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;

        public AuthService(
            UserManager<AppUser> userManager,
            IConfiguration configuration, IMapper mapper,
            IEmailConfirmTokenHelper emailConfirmToken,
            IPasswordResetTokenHelper passwordResetToken,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            IEmailSenderService emailSenderService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _emailConfirmToken = emailConfirmToken;
            _passwordResetToken = passwordResetToken;
            _emailSenderService = emailSenderService;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }



        public async Task<ServiceResponse<bool>> RegisterUser(UserRegisterRequest registerRequest)
        {

            var user = _mapper.Map<AppUser>(registerRequest);
            user.AccountCreatedDate = DateTime.Now;
            user.PwdExpiry = DateTime.Now.AddDays(90);
            user.ExpiryDate = DateTime.Now.AddYears(10);
            user.UserStatus = UserStatus.Active;
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return ServiceResponse<bool>.Failed("Please make sure to confirm your password.", null);
            }
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
            {
                var serviceResponse = ServiceResponse<bool>.Failed(
                    "Failed to create new user",
                    result.Errors.Select(e => e.Description));
                return serviceResponse;
            }
            await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
            var send_email = await SendEmailConfirmation(registerRequest.Email);
            var accessToken = await CreateAccessToken(user);
            return ServiceResponse<bool>.Succeeded(true, "User created successfully");
        }

        public Task<ServiceResponse<UserLoginResponse>> ValidateUser(UserLoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
                return ServiceResponse<UserLoginResponse>.Failed("Incorrect email", null);
            var lockOutResult = await _userManager.IsLockedOutAsync(user);

            if (_userManager.SupportsUserLockout && lockOutResult)
                return ServiceResponse<UserLoginResponse>.Failed("User is locked out. Try again after 2 minutes", null);
            if (await _userManager.CheckPasswordAsync(user,
            loginRequest.Password))
            {
                if (user.UserStatus == UserStatus.Inactive)
                    return ServiceResponse<UserLoginResponse>.Failed("User Status is Deactivated. Please Consult to your Administrator.", null);
                if (user.PwdExpiry < DateTime.Now)
                    return ServiceResponse<UserLoginResponse>.Succeeded(null, "Your password has been expired");
                if (_userManager.SupportsUserLockout && await _userManager.GetAccessFailedCountAsync(user) > 0)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                }
                var accessToken = await CreateAccessToken(user);
                var refreshToken = CreateRefreshToken();
                var userToken = new UserToken
                {
                    UserId = user.Id
                };
                userToken.UserRefreshToken = refreshToken;
                var jwtSettings = _configuration.GetSection("JwtSettings");
                userToken.RefreshTokenExpiryTime = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings.GetSection("expires").Value));
                await _db.UserRefreshTokens.AddAsync(userToken);
                await _db.SaveChangesAsync();
                await _userManager.UpdateAsync(user);
                return ServiceResponse<UserLoginResponse>.Succeeded(new UserLoginResponse
                { AccessToken = accessToken, RefreshToken = refreshToken }, "Login successful");
            }
            else
            {
                if (_userManager.SupportsUserLockout && await _userManager.GetLockoutEnabledAsync(user))
                {
                    await _userManager.AccessFailedAsync(user);
                }
                return ServiceResponse<UserLoginResponse>.Failed("Incorrect password", null);
            }
        }


        public Task<ServiceResponse<EmailConfirmResponse>> ConfirmEmail(string id, string token)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> ChangePassword(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }



        public Task<ServiceResponse<bool>> ExtendPassword(string email, ExtendPasswordExpiryRequest extendPasswordExpiryRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> ForgotPassword(string email, string userType)
        {
            throw new NotImplementedException();
        }

        public string GetUserEmail()
        {
            throw new NotImplementedException();
        }

        public int GetUserId()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<UserLoginResponse>> RefreshToken(TokenRefreshRequest tokenRefreshRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> ReSendEmailConfirmation(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> ResetPassword(UserPasswordResetRequest userPasswordResetRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> RevokeAllToken()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> RevokeLoggedInUser()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> RevokeToken(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> SendEmailConfirmation(string email)
        {
            throw new NotImplementedException();
        }

    }
}
