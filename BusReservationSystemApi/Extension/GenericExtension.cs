using BusReservationSystemApi.Data.DBContext;
using BusReservationSystemApi.Filters;
using BusReservationSystemApi.Helpers;
using BusReservationSystemApi.Helpers.Tokens;
using BusReservationSystemApi.Services.AdminServices;
using BusReservationSystemApi.Services.AuthServices;
using BusReservationSystemApi.Services.EmailSenderService;
using HashidsNet;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace BusReservationSystemApi.Extension
{
    public static class GenericExtension
    {
        public static void ConfigureOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
                //config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 };
            }).AddJsonOptions(x =>
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
                .AddXmlDataContractSerializerFormatters();
            services.AddHttpContextAccessor();
        }
        public static void ConfigureServicesDI(this IServiceCollection services, string hashId)
        {
            services.AddSingleton<IHashids>(_ => new Hashids(hashId, 11));
            services.AddHashids(setup =>
            {
                setup.Salt = hashId;
                setup.MinHashLength = 11;
            });

            //filters
            services.AddScoped<ValidationFilter>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IEmailConfirmTokenHelper, EmailConfirmTokenHelper>();
            services.AddScoped<IPasswordResetTokenHelper, PasswordResetTokenHelper>();
            services.AddScoped<TokenHelper, EmailConfirmTokenHelper>();
            services.AddScoped<TokenHelper, PasswordResetTokenHelper>();
            services.AddScoped<IHashIdHelper, HashIdHelper>();
            //services

        }

        public static void ConfigureDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        public static void ConfigureProductionDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });
        }
    }
}
