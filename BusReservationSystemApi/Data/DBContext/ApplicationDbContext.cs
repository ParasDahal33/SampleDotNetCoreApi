using BusReservationSystemApi.Data.Enumeration;
using BusReservationSystemApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BusReservationSystemApi.Data.DBContext
{
    public partial class ApplicationDbContext : IdentityDbContext<AppUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserToken>().HasKey(k => new { k.UserId, k.UserRefreshToken });
            modelBuilder.Entity<AppUser>().HasData(new AppUser()
            {
                Id = "5f1e8d3d-329e-474a-9526-1682fe508898",    // Make sure that admin user has known Id.
                UserName = "Admin",
                FullName = "Super Admin",
                ExpiryDate = DateTime.Now.AddYears(10),
                AccountCreatedDate = DateTime.Now,
                NormalizedUserName = "ADMIN".ToUpper(),
                Email = "test@gmail.com",
                UserStatus = UserStatus.Active,
                EmailConfirmed = true,
                NormalizedEmail = "TEST@GMAIL.COM",
                PhoneNumber = "0133742069",
                PasswordHash = "AQAAAAIAAYagAAAAECTYdnmMx0wFy6TQqoCjGEDTHSstulzRiwtWErtCDXf3ffYv4YqrFVkIFc5G29ZROg==",
                SecurityStamp = "VYS6BLZZ6E4F4KNOTHNXLI4ILDRSRNBG",
                PhoneNumberConfirmed = true,
                PwdExpiry = DateTime.Now.AddYears(67),
                LockoutEnabled = false,
            });

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = "0d8b99e9-8905-4e04-a1af-80d82dbebec2",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole()
            {
                Id = "198ee89a-e0df-4f24-b770-200fa6fa03d0",
                Name = "Manager",
                NormalizedName = "MANAGER"
            },
            new IdentityRole()
            {
                Id = "84d18e55-fcc0-4419-b308-8b84f293ee66",
                Name = "User",
                NormalizedName = "USER"
            },
            new IdentityRole()
            {
                Id = "fddd68cd-3c46-4657-b910-c97a14c95a28",
                Name = "Client",
                NormalizedName = "CLIENT"
            });

            // Set the role of super admin to admin.
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = "5f1e8d3d-329e-474a-9526-1682fe508898",
                RoleId = "0d8b99e9-8905-4e04-a1af-80d82dbebec2"
            });
        }

           
        public DbSet<AppUser> AppUser { get; set; }

        public virtual DbSet<UserToken> UserRefreshTokens { get; set; }

      

    }
}
