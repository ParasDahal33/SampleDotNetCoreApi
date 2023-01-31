using BusReservationSystemApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace BusReservationSystemApi.Helpers.Tokens
{
    public abstract class TokenHelper
    {

        protected TokenHelper(UserManager<AppUser> userManager) { }
        protected string TokenEncoder(string token)
        {
            var encodedToken = System.Text.Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);
            return validToken;
        }

        protected string TokenDecoder(string token)
        {
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            var normalToken = System.Text.Encoding.UTF8.GetString(decodedToken);
            return normalToken;
        }

        public abstract Task<string> GenerateToken(AppUser user);
    }
}
