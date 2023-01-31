using BusReservationSystemApi.Data.DBContext;
using BusReservationSystemApi.Extension;
using BusReservationSystemApi.Utils;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var myAllowSpecificOrigins = "AllowOrigin";
// TODO Get and set local-config for database.
var connectionString = ConfigUtils.GetConnectionString(configuration);
var Hashid =
    configuration.GetSection("Hashid").Get<string>();

// Add services to the container.
services.AddControllers();
services.AddEndpointsApiExplorer();
services.ConfigureResponseCompression();
services.ConfigureSwagger();
services.ConfigureAuth(configuration);
services.ConfigureEmailSender(configuration);
services.ConfigureOptions();

services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

});

// Add signalR to the container.
services.AddSignalR();
services.ConfigureDbContext(connectionString);
services.ConfigureServicesDI(Hashid);
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.ConfigureIdentity();
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.ConfigureCors(myAllowSpecificOrigins);
var app = builder.Build();
app.UseCors(myAllowSpecificOrigins);

// TODO Write warning.

if (configuration.GetSection("EnableSwagger").Value == "true")
{
    app.UseCustomSwagger();
}


// Configure the HTTP request pipeline.
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //if (db.Database.EnsureCreated())
    //{
    //    var dummyDataSeed = new DummyDataSeed();
    //    await dummyDataSeed.SeedDummyData(db);

    //    var seedData = new SeedData();
    //    await seedData.SeedUserData(scope.ServiceProvider);


    //}
}


app.UseResponseCompression();
// app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseForwardedHeaders();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }