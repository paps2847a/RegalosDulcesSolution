using DataLogic.Services;
using DataModel.Models;
using DataModel.Tables;
using Microsoft.AspNetCore.Mvc;

namespace Datahub.EndPoints
{
    public static class WsGroupEndPoint
    {
        public static IEndpointRouteBuilder MapWsGroupEndPoint(this IEndpointRouteBuilder EndPoint)
        {
            var EndPointData = EndPoint.MapGroup("/api/wsgroup");
            
            EndPointData.MapPost("getall", async ([FromBody] Filter data, IWsGroupService _svc) =>
            {
                var WsGroups = await _svc.GetAll(data);
                return Results.Ok(new Responses() { Data = WsGroups });
            });

            EndPointData.MapGet("get/{id:int}", async (int id, IWsGroupService _svc) =>
            {
                var WsGroup = await _svc.GetById(id);
                if (WsGroup is null)
                    return Results.Ok(new Responses() { Success = false, Message = "WsGroup not found", StatusCode = 404 });
                
                return Results.Ok(new Responses() { Data = WsGroup });
            });

            EndPointData.MapPost("new", async ([FromBody] WsGroup data, IWsGroupService _svc) =>
            {
                var WsGroup = await _svc.Add(data);
                if(WsGroup.IdGrp == 0)
                    return Results.Ok(new Responses() { Success = false, Message = "Resource not added", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = WsGroup });
            });

            EndPointData.MapPost("new-api", async ([FromBody] IEnumerable<Root> groups, IWsGroupService _svc) =>
            {
                var WsGroup = await _svc.AddGroupsPayLoad(groups);
                if (!WsGroup)
                    return Results.Ok(new Responses() { Success = false, Message = "Resource not added", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = WsGroup });
            });

            EndPointData.MapPost("upd", async ([FromBody] WsGroup data, IWsGroupService _svc) =>
            {
                var WsGroup = await _svc.Upd(data);
                if (WsGroup.IdGrp == 0)
                    return Results. Ok(new Responses() { Success = false, Message = "Resource not updated", StatusCode = 404 });

                return Results.Ok(new Responses() { Data = WsGroup });
            });

            EndPointData.MapPost("del", async ([FromBody] WsGroup data, IWsGroupService _svc) =>
            {
                var result = await _svc.Del(data);
                if (!result)
                    return Results.NotFound(new Responses() { Success = false, Message = "Resource not deleted", StatusCode = 404 });
                return Results.Ok(new Responses() { Data = data });
            });

            EndPointData.MapGet("count", async (IWsGroupService _svc) => new Responses() { Data = await _svc.Count() });

            return EndPoint;
        }
    }
}
