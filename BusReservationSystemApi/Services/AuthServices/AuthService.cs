using AutoMapper;
using BusReservationSystemApi.Data.Configuration;
using BusReservationSystemApi.Data.DBContext;
using BusReservationSystemApi.Data.DTO.Request;
using BusReservationSystemApi.Data.DTO.Response;
using BusReservationSystemApi.Data.Enumeration;
using BusReservationSystemApi.Data.Models;
using BusReservationSystemApi.Helpers.Tokens;
using BusReservationSystemApi.Services.EmailSenderService;
using BusReservationSystemApi.Utils;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

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

        public async Task<ServiceResponse<UserLoginResponse>> ValidateUser(UserLoginRequest loginRequest)
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


        public async Task<ServiceResponse<EmailConfirmResponse>> ConfirmEmail(string id, string token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(id))
                return ServiceResponse<EmailConfirmResponse>.Failed("something is wrong with the link. Try it again", null);

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return ServiceResponse<EmailConfirmResponse>.Failed("User not found", null);

            if (user.EmailConfirmed && user.PasswordHash != null)
            {
                return ServiceResponse<EmailConfirmResponse>.Failed("Email already confirmed!", null);
            }
            if (await _emailConfirmToken.ConfirmEmailAction(user, token))
            {
                var result = new EmailConfirmResponse
                {
                    Id = id,
                    ResetToken = await _userManager.GeneratePasswordResetTokenAsync(user)
                };
                return ServiceResponse<EmailConfirmResponse>.Succeeded(result, "Email has been confirmed.");
            }

            return ServiceResponse<EmailConfirmResponse>.Failed("Failed to confirm email.", null);

        }


        public async Task<ServiceResponse<bool>> ResetPassword(UserPasswordResetRequest userPasswordResetRequest)
        {
            var user = await _userManager.FindByIdAsync(userPasswordResetRequest.Id);
            if (user is null)
                return ServiceResponse<bool>.Failed("User not found", null);
            if (user.UserStatus == UserStatus.Inactive)
            {
                user.UserStatus = UserStatus.Active;
                user.PwdExpiry = DateTime.Now.AddMonths(6);
                _db.AppUser.Update(user);
            }
            user.PasswordChangeDate = DateTime.Now;
            var result = await _passwordResetToken.ResetPasswordAction(user, userPasswordResetRequest.Token, userPasswordResetRequest.ConfirmPassword);
            return result;
        }


        public async Task<ServiceResponse<bool>> ExtendPassword(string email, ExtendPasswordExpiryRequest extendPasswordExpiryRequest)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return ServiceResponse<bool>.Failed("User not found", null);
            if (user.PwdExpiry < DateTime.Now)
            {
                user.PwdExpiry = DateTime.Now.AddMonths(6);
                user.PasswordChangeDate = DateTime.Now;
                var resultOne = await _userManager.UpdateAsync(user);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, code, extendPasswordExpiryRequest.Password);
                if (result.Succeeded)
                {
                    return ServiceResponse<bool>.Succeeded(true, "Password Extended Successfully");
                }
                else
                {
                    return ServiceResponse<bool>.Failed("Attempt Unsuccessful", null);
                }
            }
            return ServiceResponse<bool>.Failed("Attempt Unsuccessful", null);
        }


        public async Task<ServiceResponse<bool>> ForgotPassword(string email, string userType)
        {
            if (string.IsNullOrWhiteSpace(email))
                return ServiceResponse<bool>.Failed("Email is empty", null);
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return ServiceResponse<bool>.Failed("User not found", null);

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = CreateUrlLink(user.Id, token, UrlActions.resetpassword.ToString());

            var mailAddress = new EmailAddress { DisplayName = user.UserName, Address = user.Email };
            var message = new Message(new EmailAddress[] { mailAddress }, "Reset your password", Constants.GetForgetPasswordHtml(user.UserName, url), null);

            await _emailSenderService.SendEmailAsync(message);
            return ServiceResponse<bool>.Succeeded(true, "Check your email to reset the password");

        }

        public async Task<ServiceResponse<bool>> ChangePassword(ChangePasswordRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (request.Password != request.ConfirmPassword)
            {
                return ServiceResponse<bool>.Failed("Incorrect Confirm password", null);
            }
            if (user is null)
                return ServiceResponse<bool>.Failed("User not found", null);
            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);
            if (result.Succeeded)
            {
                return ServiceResponse<bool>.Succeeded(true, "Password Changed Successfully");
            }
            else
            {
                return ServiceResponse<bool>.Failed("Attempt Unsuccessful", null);
            }
        }


        public async Task<ServiceResponse<bool>> SendEmailConfirmation(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return ServiceResponse<bool>.Failed("Email is empty", null);
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return ServiceResponse<bool>.Failed("User not found", null);

            var token = await _emailConfirmToken.GenerateToken(user);
            var url = CreateUrlLink(user.Id, token, UrlActions.confirmEmail.ToString());
            var mailAddress = new EmailAddress { DisplayName = user.UserName, Address = user.Email };
            var message = new Message(new EmailAddress[] { mailAddress }, "Email Confirmation", Constants.GetConfirmEmailHtml(user.UserName, url), null);
            await _emailSenderService.SendEmailAsync(message);
            return ServiceResponse<bool>.Succeeded(true, "Check your email to verify.");
        }

        public string GetUserEmail() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        public int GetUserId() => int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));


        public async Task<ServiceResponse<UserLoginResponse>> RefreshToken(TokenRefreshRequest tokenRefreshRequest)
        {

            var currentToken = _db.UserRefreshTokens.FirstOrDefault(t => t.UserRefreshToken == tokenRefreshRequest.RefreshToken);


            if (currentToken == null)
            {
                return ServiceResponse<UserLoginResponse>.Failed("Invalid token.", null);
            }

            var userId = currentToken.UserId;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return ServiceResponse<UserLoginResponse>.Failed("Invalid token.", null);
            }


            if (currentToken.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return ServiceResponse<UserLoginResponse>.Failed("Token already expired.", null);
            }

            var newAccessToken = await CreateAccessToken(user);
            var newRefreshToken = CreateRefreshToken();

            // Add the new refresh token to te database.
            var newUserToken = new UserToken
            {
                UserId = user.Id
            };

            var jwtSettings = _configuration.GetSection("JwtSettings");

            newUserToken.UserRefreshToken = newRefreshToken;
            newUserToken.RefreshTokenExpiryTime = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings.GetSection("expires").Value));

            // TODO try catch
            using var transaction = _db.Database.BeginTransaction();
            _db.UserRefreshTokens.Add(newUserToken);
            // Remove the old token from the database.
            _db.UserRefreshTokens.Remove(currentToken);
            _db.SaveChanges();
            await transaction.CommitAsync();

            await _userManager.UpdateAsync(user);

            return ServiceResponse<UserLoginResponse>.Succeeded(new UserLoginResponse
            { AccessToken = newAccessToken, RefreshToken = newRefreshToken }, "Token Refreshed successfully.");

        }

        public async Task<ServiceResponse<bool>> ReSendEmailConfirmation(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null)
                return ServiceResponse<bool>.Failed("User not found", null);
            if (user.EmailConfirmed == true)
            {
                return ServiceResponse<bool>.Failed("User Email already confirmed.", null);
            }
            var result = await SendEmailConfirmation(user.Email);
            return ServiceResponse<bool>.Succeeded(true, "Confirmation email sent successfully.");
        }

        public async Task<ServiceResponse<bool>> RevokeLoggedInUser()
        {
            var userEmail = GetUserEmail();
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null)
                return ServiceResponse<bool>.Failed("User not found", null);

            // Delete all the tokens associated with the user from the database.
            var currentUserTokens = _db.UserRefreshTokens.Where(u => u.UserId == user.Id);
            _db.UserRefreshTokens.RemoveRange(currentUserTokens);
            await _db.SaveChangesAsync();
            return ServiceResponse<bool>.Succeeded(true, "User token revoked successfully");
        }

        public async Task<ServiceResponse<bool>> RevokeToken(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return ServiceResponse<bool>.Failed("User not found", null);

            // Delete all the tokens associated with the user from the database.
            var currentUserTokens = _db.UserRefreshTokens.Where(u => u.UserId == user.Id);
            _db.UserRefreshTokens.RemoveRange(currentUserTokens);
            await _db.SaveChangesAsync();
            return ServiceResponse<bool>.Succeeded(true, "User token revoked successfully");
        }

        public async Task<ServiceResponse<bool>> RevokeAllToken()
        {
            _db.UserRefreshTokens.RemoveRange(_db.UserRefreshTokens);
            await _db.SaveChangesAsync();
            return ServiceResponse<bool>.Succeeded(true, "All User Token revoked successfully.");
        }


    }
}
