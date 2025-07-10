using Datahub.Extensions;
using Datahub.Routes;

namespace Datahub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateSlimBuilder(args);

            builder.Services.AddDataContext(builder.Configuration)
                            .AddServices()
                            .AddCustomMvcServices(builder.Configuration);

            var app = builder.Build();

            app.MapCategoriaRoute()
                .MapTamanoRoute()
                .MapInventarioRoute();

            app.Run();
        }
    }
}
