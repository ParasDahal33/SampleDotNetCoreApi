using BusReservationSystemApi.Data.Models;
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

        }

           
        public DbSet<AppUser> AppUser { get; set; }

        public virtual DbSet<UserToken> UserRefreshTokens { get; set; }

      

    }
}
