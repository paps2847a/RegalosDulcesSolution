using Datahub.BackgroundJobs;
using DataLogic.Services;
using DataPersistance;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using System.Runtime.CompilerServices;

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

            services.AddHybridCache(options =>
            {
                options.DefaultEntryOptions = new HybridCacheEntryOptions
                {
                    LocalCacheExpiration = TimeSpan.FromMinutes(3),
                };
            });

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

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration _config)
        {
            services.AddHostedService<PeriodicMsgBackground>();

            services.AddScoped<ITamanoService, TamanoService>();
            services.AddScoped<IInventarioService, InventarioService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IWsGroupService, WsGroupService>();
            services.AddScoped<IMensajeService, MensajeService>();
            services.AddScoped<IRecordatorioService, RecordatorioService>();

            var url = _config.GetValue<string>("ApiBotUrl");
            if(string.IsNullOrEmpty(url))
                throw new ArgumentNullException("No se encontró la URL del bot en la configuración");

            services.AddHttpClient("DatahubWsBot", (client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(40);
                client.BaseAddress = new Uri(url);
            });

            return services;
        }

    }
}
