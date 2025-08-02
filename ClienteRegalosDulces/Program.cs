using ClienteRegalosDulces.Services;
using Polly;
using Polly.Extensions.Http;

namespace ClienteRegalosDulces
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.
                AddMVCServices()
                .ClientServices();

            var app = builder.Build();

            app.UseRouting();
            app.UseHttpsRedirection();
            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }

    public static class StaticAssetsExtensions
    {
        public static IServiceCollection ClientServices(this IServiceCollection services)
        {
            services.AddHttpClient<ITamanoService, TamanoService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<ICategoriaService, CategoriaService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<IInventarioService, InventarioService>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        public static IServiceCollection AddMVCServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddControllersWithViews()
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            return services;
        }

        // Política de reintentos
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        // Política de Circuit Breaker
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                .CircuitBreakerAsync(4, TimeSpan.FromSeconds(30));
        }
    }
}
