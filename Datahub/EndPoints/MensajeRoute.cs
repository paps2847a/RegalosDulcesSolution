using DataLogic.Services;
using DataModel.Models;
using DataModel.Tables;
using Microsoft.AspNetCore.Mvc;

namespace Datahub.EndPoints
{
    public static class MensajeEndPoint
    {
        public static IEndpointRouteBuilder MapMensajeEndPoint(this IEndpointRouteBuilder EndPoint)
        {
            var EndPointData = EndPoint.MapGroup("/api/mensaje");
            
            EndPointData.MapPost("getall", async ([FromBody] Filter data, IMensajeService _svc) =>
            {
                var Mensajes = await _svc.GetAll(data);
                return Results.Ok(new Responses() { Data = Mensajes });
            });

            EndPointData.MapGet("get/{id:int}", async (int id, IMensajeService _svc) =>
            {
                var Mensaje = await _svc.GetById(id);
                if (Mensaje is null)
                    return Results.Ok(new Responses() { Success = false, Message = "Mensaje not found", StatusCode = 404 });
                
                return Results.Ok(new Responses() { Data = Mensaje });
            });

            EndPointData.MapPost("new", async ([FromBody] Mensaje data, IMensajeService _svc) =>
            {
                var Mensaje = await _svc.Add(data);
                if(Mensaje.IdMsg == 0)
                    return Results.Ok(new Responses() { Success = false, Message = "Resource not added", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = Mensaje });
            });

            EndPointData.MapPost("upd", async ([FromBody] Mensaje data, IMensajeService _svc) =>
            {
                var Mensaje = await _svc.Upd(data);
                if (Mensaje.IdMsg == 0)
                    return Results. Ok(new Responses() { Success = false, Message = "Resource not updated", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = Mensaje });
            });

            EndPointData.MapPost("del", async ([FromBody] Mensaje data, IMensajeService _svc) =>
            {
                var result = await _svc.Del(data);
                if (!result)
                    return Results.NotFound(new Responses() { Success = false, Message = "Resource not deleted", StatusCode = 404 });
                return Results.Ok(new Responses() { Data = data });
            });

            EndPointData.MapGet("count", async (IMensajeService _svc) => new Responses() { Data = await _svc.Count() });

            return EndPoint;
        }
    }
}
