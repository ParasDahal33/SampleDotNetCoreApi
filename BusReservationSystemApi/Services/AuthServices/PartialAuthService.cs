

using BusReservationSystemApi.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BusReservationSystemApi.Services.AuthServices
{
    public partial class AuthService
    {
        public async Task<string> CreateAccessToken(AppUser user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        }

        private string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            // TODO Reverse to minutes
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
             (
                 issuer: jwtSettings.GetSection("ValidIssuer").Value,
                 audience: jwtSettings.GetSection("ValidAudience").Value,
                 claims: claims,
                 expires: DateTime.Now.AddSeconds(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
                signingCredentials: signingCredentials
             );
            return tokenOptions;
        }

        private async Task<List<Claim>> GetClaims(AppUser user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.FullName)
         };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key =
                        System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private string CreateUrlLink(string id, string token, string action)
        {
            string link = $"{_configuration["AppUrl"]}/login/{action}?id={id}&token={token}";
            return link;
        }
    }

}
