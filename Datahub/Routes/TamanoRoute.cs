using DataLogic.Services;
using DataModel.Models;
using DataModel.Tables;
using Microsoft.AspNetCore.Mvc;

namespace Datahub.Routes
{
    public static class TamanoRoute
    {

        public static IEndpointRouteBuilder MapTamanoRoute(this IEndpointRouteBuilder route)
        {
            var routeData = route.MapGroup("/api/tamano");
            
            routeData.MapPost("getall", async ([FromBody] Filter data, ITamanoService _svc) =>
            {
                var Tamanos = await _svc.GetAll(data);
                return Results.Ok(new Responses() { Data = Tamanos });
            });

            routeData.MapGet("get/{id:int}", async (int id, ITamanoService _svc) =>
            {
                var Tamano = await _svc.GetById(id);
                if (Tamano is null)
                    return Results.Ok(new Responses() { Success = false, Message = "Tamano not found", StatusCode = 404 });
                
                return Results.Ok(new Responses() { Data = Tamano });
            });

            routeData.MapPost("new", async ([FromBody] Tamano data, ITamanoService _svc) =>
            {
                var Tamano = await _svc.Add(data);
                if(Tamano.IdTam == 0)
                    return Results.Ok(new Responses() { Success = false, Message = "Resource not added", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = Tamano });
            });

            routeData.MapPost("upd", async ([FromBody] Tamano data, ITamanoService _svc) =>
            {
                var Tamano = await _svc.Upd(data);
                if (Tamano.IdTam == 0)
                    return Results. Ok(new Responses() { Success = false, Message = "Resource not updated", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = Tamano });
            });

            routeData.MapPost("del", async ([FromBody] Tamano data, ITamanoService _svc) =>
            {
                var result = await _svc.Del(data);
                if (!result)
                    return Results.NotFound(new Responses() { Success = false, Message = "Resource not deleted", StatusCode = 404 });
                return Results.Ok(new Responses() { Data = data });
            });

            routeData.MapGet("count", async (ITamanoService _svc) => new Responses() { Data = await _svc.Count() });

            return routeData;
        }
    }
}
