namespace BusReservationSystemApi.Extension
{
    public static class CorsExtension
    {

        // TODO Configure cors properly.
        public static void ConfigureCors(this IServiceCollection services, string myAllowSpecificOrigins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(myAllowSpecificOrigins,
                    builder => builder
                    .WithOrigins("http://127.0.0.1:5173", "http://192.168.0.137:1337", "*", "https://portal.mbnepal.com", "http://portal.mbnepal.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }
    }
}
