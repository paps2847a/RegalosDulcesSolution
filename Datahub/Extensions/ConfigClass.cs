

using DataLogic.Services;
using DataPersistance;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Datahub.Extensions
{
    public static class ConfigClass
    {
        public static IServiceCollection AddDataContext(this IServiceCollection data, IConfiguration config)
        {
            data.AddDbContextPool<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
            });
            return data;
        }

        public static IServiceCollection AddCustomMvcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddHsts(options =>
            {
                options.IncludeSubDomains = true; //true,if we need to include subdomains
                options.MaxAge = TimeSpan.FromDays(365);//specify days
            });

            services.Configure<HostOptions>(options =>
            {
                options.ServicesStartConcurrently = true;
                options.ServicesStopConcurrently = true;
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ITamanoService, TamanoService>();
            services.AddSingleton<IInventarioService, InventarioService>();
            services.AddSingleton<ICategoriaService, CategoriaService>();

            return services;
        }

    }
}
