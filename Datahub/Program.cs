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
                            .AddCustomMvcServices(builder.Configuration)
                            .AddServices();

            var app = builder.Build();

            app.MapCategoriaEndPoint()
               .MapInventarioEndPoint()
               .MapWsGroupEndPoint()
               .MapTamanoEndPoint();

            app.Run();
        }
    }
}
