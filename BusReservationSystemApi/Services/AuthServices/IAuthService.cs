using BusReservationSystemApi.Data.Configuration;
using BusReservationSystemApi.Data.DTO.Request;
using BusReservationSystemApi.Data.DTO.Response;
using BusReservationSystemApi.Data.Models;

namespace BusReservationSystemApi.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ServiceResponse<bool>> RegisterUser(UserRegisterRequest registerRequest);
        Task<ServiceResponse<UserLoginResponse>> ValidateUser(UserLoginRequest loginRequest);
        Task<ServiceResponse<EmailConfirmResponse>> ConfirmEmail(string id, string token);
        Task<ServiceResponse<bool>> ChangePassword(ChangePasswordRequest request);
        Task<ServiceResponse<bool>> ExtendPassword(string email, ExtendPasswordExpiryRequest extendPasswordExpiryRequest);
        Task<ServiceResponse<bool>> ResetPassword(UserPasswordResetRequest userPasswordResetRequest);
        Task<ServiceResponse<bool>> ForgotPassword(string email, string userType);
        Task<ServiceResponse<bool>> SendEmailConfirmation(string email);
        Task<string> CreateAccessToken(AppUser user);
        Task<ServiceResponse<UserLoginResponse>> RefreshToken(TokenRefreshRequest tokenRefreshRequest);
        Task<ServiceResponse<bool>> RevokeLoggedInUser();
        Task<ServiceResponse<bool>> ReSendEmailConfirmation(string Id);
        Task<ServiceResponse<bool>> RevokeToken(string userId);
        Task<ServiceResponse<bool>> RevokeAllToken();
        string GetUserEmail();
        int GetUserId();

    }
}
