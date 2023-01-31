
using AutoMapper;
using BusReservationSystemApi.Data.Configuration;
using BusReservationSystemApi.Data.DBContext;
using BusReservationSystemApi.Data.DTO.Request;
using BusReservationSystemApi.Data.DTO.Response;
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



        public Task<ServiceResponse<bool>> RegisterUser(UserRegisterRequest registerRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<UserLoginResponse>> ValidateUser(UserLoginRequest loginRequest)
        {
            throw new NotImplementedException();
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
