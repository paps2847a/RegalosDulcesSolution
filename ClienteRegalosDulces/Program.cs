namespace ClienteRegalosDulces
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            var app = builder.Build();

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
