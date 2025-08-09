using Datahub.Helpers;
using DataLogic.Services;
using DataModel.Models;
using Microsoft.Extensions.Caching.Hybrid;
using System.Text.Json;

namespace Datahub.BackgroundJobs
{
    public class PeriodicMsgBackground : BackgroundService
    {
        private readonly IServiceProvider _svcProv;

        public PeriodicMsgBackground(IServiceProvider serviceProvider)
        {
            _svcProv = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var _cache = _svcProv.GetRequiredService<HybridCache>();
            var _client = _svcProv.GetRequiredService<IHttpClientFactory>().CreateClient("DatahubWsBot");
            var _scope = _svcProv.CreateScope();

            await Task.Delay(5000, stoppingToken); // Initial delay to ensure services are ready

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var sendMsgData = await _cache.GetOrCreateAsync("remindData", async (x) => {
                        var recordatorio = _scope.ServiceProvider.GetRequiredService<IRecordatorioService>();
                        var groupsData = _scope.ServiceProvider.GetRequiredService<IWsGroupService>();
                        var returnedData = new List<SendDataGroupMsg>();

                        var r = await recordatorio.GetAll(new Filter());
                        foreach (var item in r)
                        {
                            var results = await groupsData.GetAll(new Filter()
                            {
                                parameters = {
                                    { "op", "GetGroupsById" }, { "data", item.IdGrps }
                                }
                            });

                            if(!results.Any())
                                continue;

                            returnedData.Add(new SendDataGroupMsg()
                            {
                                GroupsIds = string.Join('|', results.Select(x => x.IdWsGrp)),
                                Msg = item.Mensaje.DesMsg,
                                HourRecord = item.HourRecord
                            });
                        }

                        return returnedData;
                    });

                    var dateData = DateTime.Now;

                    // Agregar verificacion de que se envio una sola vez
                    //ERROR EN ESTA SECCION, SOLO SE DEBEN ENVIAR UNA VEZ Y YA
                    var toSendMsg = sendMsgData.Where(x => x.HourRecord.Hours == dateData.Hour
                    && x.HourRecord.Minutes == dateData.Minute).ToArray();
                    if (toSendMsg.Length == 0)
                        continue;

                    foreach (var item in toSendMsg)
                    {
                        var data = JsonSerializer.Serialize(new { groupsIds = item.GroupsIds, msg = item.Msg });
                        var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                        var r = await _client.PostAsync("sendwsmsg", content);

                        if (!r.IsSuccessStatusCode)
                            Console.WriteLine("Mensaje no enviado");
                    }

                    int timetoWait = 60 - DateTime.Now.Second;
                    await Task.Delay(timetoWait * 1000, stoppingToken); // Initial delay to ensure services are ready
                }
                catch (Exception ex)
                {
                    ex.LogEx();
                    break;
                }
            }
        }


    }
}
