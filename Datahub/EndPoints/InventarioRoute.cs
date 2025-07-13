using DataLogic.Services;
using DataModel.Models;
using DataModel.Tables;
using Microsoft.AspNetCore.Mvc;

namespace Datahub.EndPoints
{
    public static class InventarioEndPoint
    {
        public static IEndpointRouteBuilder MapInventarioEndPoint(this IEndpointRouteBuilder EndPoint)
        {
            var EndPointData = EndPoint.MapGroup("/api/inventario");
            
            EndPointData.MapPost("getall", async ([FromBody] Filter data, IInventarioService _svc) =>
            {
                var Inventarios = await _svc.GetAll(data);
                return Results.Ok(new Responses() { Data = Inventarios });
            });

            EndPointData.MapGet("get/{id:int}", async (int id, IInventarioService _svc) =>
            {
                var Inventario = await _svc.GetById(id);
                if (Inventario is null)
                    return Results.Ok(new Responses() { Success = false, Message = "Inventario not found", StatusCode = 404 });
                
                return Results.Ok(new Responses() { Data = Inventario });
            });

            EndPointData.MapPost("new", async ([FromBody] Inventario data, IInventarioService _svc) => 
            {
                var Inventario = await _svc.Add(data);
                if(Inventario.IdInv == 0)
                    return Results.Ok(new Responses() { Success = false, Message = "Resource not added", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = Inventario });
            });

            EndPointData.MapPost("upd", async ([FromBody] Inventario data, IInventarioService _svc) =>
            {
                var Inventario = await _svc.Upd(data);
                if (Inventario.IdInv == 0)
                    return Results. Ok(new Responses() { Success = false, Message = "Resource not updated", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = Inventario });
            });

            EndPointData.MapPost("del", async ([FromBody] Inventario data, IInventarioService _svc) =>
            {
                var result = await _svc.Del(data);
                if (!result)
                    return Results.NotFound(new Responses() { Success = false, Message = "Resource not deleted", StatusCode = 404 });
                return Results.Ok(new Responses() { Data = data });
            });

            EndPointData.MapGet("count", async (IInventarioService _svc) => new Responses() { Data = await _svc.Count() });

            return EndPoint;
        }
    }
}
