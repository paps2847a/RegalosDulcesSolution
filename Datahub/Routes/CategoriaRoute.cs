using DataLogic.Services;
using DataModel.Models;
using DataModel.Tables;
using Microsoft.AspNetCore.Mvc;

namespace Datahub.Routes
{
    public static class CategoriaRoute
    {

        public static IEndpointRouteBuilder MapCategoriaRoute(this IEndpointRouteBuilder route)
        {
            var routeData = route.MapGroup("/api/categoria");
            
            routeData.MapPost("getall", async ([FromBody] Filter data, ICategoriaService _svc) =>
            {
                var categorias = await _svc.GetAll(data);
                return Results.Ok(new Responses() { Data = categorias });
            });

            routeData.MapGet("get/{id:int}", async (int id, ICategoriaService _svc) =>
            {
                var categoria = await _svc.GetById(id);
                if (categoria is null)
                    return Results.Ok(new Responses() { Success = false, Message = "Categoria not found", StatusCode = 404 });
                
                return Results.Ok(new Responses() { Data = categoria });
            });

            routeData.MapPost("new", async ([FromBody] Categoria data, ICategoriaService _svc) =>
            {
                var categoria = await _svc.Add(data);
                if(categoria.IdCat == 0)
                    return Results.Ok(new Responses() { Success = false, Message = "Resource not added", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = categoria });
            });

            routeData.MapPost("upd", async ([FromBody] Categoria data, ICategoriaService _svc) =>
            {
                var categoria = await _svc.Upd(data);
                if (categoria.IdCat == 0)
                    return Results. Ok(new Responses() { Success = false, Message = "Resource not updated", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = categoria });
            });

            routeData.MapPost("del", async ([FromBody] Categoria data, ICategoriaService _svc) =>
            {
                var result = await _svc.Del(data);
                if (!result)
                    return Results.NotFound(new Responses() { Success = false, Message = "Resource not deleted", StatusCode = 404 });
                return Results.Ok(new Responses() { Data = data });
            });

            routeData.MapGet("count", async (ICategoriaService _svc) => new Responses() { Data = await _svc.Count() });

            return routeData;
        }
    }
}
