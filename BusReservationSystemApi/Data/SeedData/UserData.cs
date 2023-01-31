
using BusReservationSystemApi.Data.Enumeration;
using BusReservationSystemApi.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace BusReservationSystemApi.Data.SeedData
{

    public static class UserData
    {

        private static PasswordHasher<AppUser> _passwordHasher = new PasswordHasher<AppUser>();

        public static readonly List<AppUser> USER_LIST = new List<AppUser> {
               new()  {
                Id = "5f1e8d3d-329e-474a-9526-1682fe508898",    // Make sure that admin user has known Id.
                UserName = "Admin",
                FullName = "Admin Admin",
                ExpiryDate = DateTime.Now.AddYears(10),
                AccountCreatedDate = DateTime.Now,
                NormalizedUserName = "ADMIN".ToUpper(),
                Email = "test@gmail.com",
                UserStatus = UserStatus.Active,
                EmailConfirmed = true,
                NormalizedEmail = "TEST@GMAIL.COM",
                PhoneNumber = "0133742069",
                PhoneNumberConfirmed = true,
                PwdExpiry = DateTime.Now.AddYears(67),
                LockoutEnabled = false,
            }
     };
    };

}
