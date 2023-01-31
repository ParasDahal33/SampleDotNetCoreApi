using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace BusReservationSystemApi.Extension
{
    public static class ResponseCompressionExtension
    {
        public static void ConfigureResponseCompression(this IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
        }
    }
}
