using Datahub.EndPoints;
using Datahub.Extensions;

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

            app.MapCategoriaEndPoint()
                .MapInventarioEndPoint()
                .MapTamanoEndPoint();

            app.Run();
        }
    }
}
