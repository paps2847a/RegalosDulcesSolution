using Datahub.Helpers;
using DataLogic.Services;
using DataModel.Models;
using DataModel.Tables;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Datahub.EndPoints
{
    public static class RecordatorioEndPoint
    {
        public static IEndpointRouteBuilder MapRecordatorioEndPoint(this IEndpointRouteBuilder EndPoint)
        {
            var EndPointData = EndPoint.MapGroup("/api/recordatorio");
            
            EndPointData.MapPost("getall", async ([FromBody] Filter data, IRecordatorioService _svc) =>
            {
                var Recordatorios = await _svc.GetAll(data);
                return Results.Ok(new Responses() { Data = Recordatorios });
            });

            EndPointData.MapPost("sendtestmsg", async ([FromBody] Filter data, IWsGroupService _svcWsGroup, IHttpClientFactory _factory) =>
            {
                try
                {
                    var grupos = await _svcWsGroup.GetAll(data);
                    if (!grupos.Any())
                        return Results.NotFound(new Responses { Success = false, Message = "No se encontraron grupos", StatusCode = 404 });

                    if (!data.parameters.TryGetValue("msg", out var msgObj) || msgObj is null)
                        return Results.BadRequest(new Responses { Success = false, Message = "Falta el parámetro 'msg'", StatusCode = 400 });

                    var client = _factory.CreateClient("DatahubWsBot");
                    var groupsIdsStr = string.Join('|', grupos.Select(x => x.IdWsGrp));
                    var payload = new { groupsIds = groupsIdsStr, msg = msgObj.ToString() };
                    var strCont = JsonSerializer.Serialize(payload);

                    var response = await client.PostAsync("sendwsmsg", new StringContent(strCont, System.Text.Encoding.UTF8, "application/json"));
                    if (!response.IsSuccessStatusCode)
                        return Results.BadRequest(new Responses { Success = false, Message = "Error al enviar el mensaje", StatusCode = 400 });

                    return Results.Ok(new Responses { Success = true, Message = "Éxito al enviar mensaje" });
                }
                catch (Exception ex)
                {
                    ex.LogEx();
                    return Results.BadRequest(new Responses { Success = false, Message = "Error inesperado al enviar el mensaje", StatusCode = 500 });
                }
            });

            EndPointData.MapGet("get/{id:int}", async (int id, IRecordatorioService _svc) =>
            {
                var Recordatorio = await _svc.GetById(id);
                if (Recordatorio is null)
                    return Results.Ok(new Responses() { Success = false, Message = "Recordatorio not found", StatusCode = 404 });
                
                return Results.Ok(new Responses() { Data = Recordatorio });
            });

            EndPointData.MapPost("new", async ([FromBody] Recordatorio data, IRecordatorioService _svc) =>
            {
                var Recordatorio = await _svc.Add(data);
                if(Recordatorio.IdRecord == 0)
                    return Results.Ok(new Responses() { Success = false, Message = "Resource not added", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = Recordatorio });
            });

            EndPointData.MapPost("upd", async ([FromBody] Recordatorio data, IRecordatorioService _svc) =>
            {
                var Recordatorio = await _svc.Upd(data);
                if (Recordatorio.IdRecord == 0)
                    return Results. Ok(new Responses() { Success = false, Message = "Resource not updated", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = Recordatorio });
            });

            EndPointData.MapPost("del", async ([FromBody] Recordatorio data, IRecordatorioService _svc) =>
            {
                var result = await _svc.Del(data);
                if (!result)
                    return Results.NotFound(new Responses() { Success = false, Message = "Resource not deleted", StatusCode = 404 });
                return Results.Ok(new Responses() { Data = data });
            });

            EndPointData.MapGet("count", async (IRecordatorioService _svc) => new Responses() { Data = await _svc.Count() });

            return EndPoint;
        }
    }
}
