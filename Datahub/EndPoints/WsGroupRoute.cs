using Datahub.Helpers;
using DataLogic.Services;
using DataModel.Models;
using DataModel.Tables;
using Microsoft.AspNetCore.Mvc;

namespace Datahub.EndPoints
{
    public static class WsGroupEndPoint
    {
        public static async Task<IEnumerable<Root>> RequestToBot(IHttpClientFactory _factory, IConfiguration _config)
        {
            try
            {
                var client = _factory.CreateClient("DatahubWsBot");

                var url = _config.GetValue<string>("ApiBotUrl");
                if (string.IsNullOrEmpty(url))
                    return [];

                var response = await client.GetAsync($"{url}getusergroups");
                if (!response.IsSuccessStatusCode)
                    return [];

                var content = await response.Content.ReadFromJsonAsync<List<Root>>();
                if (content is null || !content.Any())
                    return [];

                return content;
            }
            catch (Exception ex)
            {
                ex.LogEx();
                return [];
            }
        }

        public static IEndpointRouteBuilder MapWsGroupEndPoint(this IEndpointRouteBuilder EndPoint)
        {
            var EndPointData = EndPoint.MapGroup("/api/wsgroup");
            
            EndPointData.MapPost("getall", async ([FromBody] Filter data, IWsGroupService _svc) =>
            {
                var WsGroups = await _svc.GetAll(data);
                return Results.Ok(new Responses() { Data = WsGroups });
            });

            EndPointData.MapGet("sync", async (IHttpClientFactory _factory, IWsGroupService _svc, IConfiguration _config) =>
            {
                try
                {
                    var groupsData = await RequestToBot(_factory, _config);
                    if(!groupsData.Any())
                        return Results.Ok(new Responses() { Success = false, Message = "WsGroup not found", StatusCode = 404 });

                    var registeredGroups = await _svc.GetAll(new Filter()
                    {
                        parameters = { { "op", "GetRegisteredGroups" }, { "data", string.Join('|', groupsData.Select(x => x.id._serialized)) } }
                    });
                    if (!registeredGroups.Any())
                        return Results.Ok(new Responses() { Success = false, Message = "WsGroup not found", StatusCode = 404 });

                    var newGroups = groupsData
                        .Where(x => !registeredGroups.Any(c => c.IdWsGrp == x.id._serialized))
                        .Select(x => new WsGroup()
                        {
                            IdWsGrp = x.id._serialized,
                            GrpNam = x.name.ToString() ?? ""
                        });

                    if(!newGroups.Any())
                        return Results.Ok(new Responses() { Message = "No Groups to Sync"});

                    var r = await _svc.AddRange(newGroups);
                    if(!r)
                        return Results.Ok(new Responses() { Success = false, Message = "WsGroup not found", StatusCode = 404 });

                    return Results.Ok(new Responses() { Message = "Grupos Sincronizados" });
                }
                catch(Exception ex)
                {
                    ex.LogEx();
                    return Results.Ok(new Responses() { Success = false, Message = "Error al obrener los grupos" });
                }
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
