using ClienteRegalosDulces.Services;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using System.ComponentModel;
using DataModel.Tables;
using DataModel.Models;
using ClienteRegalosDulces.Helpers;

namespace ClienteRegalosDulces.Controllers
{
    [DisplayName("Manejo de Usuarios")]
    public class RecordatorioController : Controller
    {
        private readonly IRecordatorioService _svc;
        private readonly IWsGroupService _svcWsGrp;
        private readonly IMensajeService _svcMsg;

        public RecordatorioController(IRecordatorioService RecordatorioService, IWsGroupService svcWsGrp, IMensajeService msgSvc)
        {
            _svc = RecordatorioService ?? throw new ArgumentNullException(nameof(RecordatorioService));
            _svcWsGrp = svcWsGrp;
            _svcMsg = msgSvc;
        }

        [DisplayName("Pantalla de Inicio")]
        public IActionResult Index()
        {
            return View();
        }

        [DisplayName("Pantalla de Inicio")]
        public async Task<IActionResult> Crud(int id)
        {
            ViewBag.groups = await _svcWsGrp.GetAllAsync(new Filter());
            return View(id > 0 ? await _svc.GetItem(id) : new Recordatorio());
        }

        [HttpPost]
        [DisplayName("Acceder a Datos")]
        public async Task<IActionResult> SendTestMessage([FromBody] Filter data)
        {
            try
            {
                var r = await _svc.SendMsgTest(data);
                return Ok(new { rs = r.Success ? "ok" : "fail", msg = r.Message });
            }
            catch (Exception ex)
            {
                ex.LogEx();
                HandleBrokenCircuitException();
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [DisplayName("Acceder a Datos")]
        public async Task<IActionResult> GetData([FromBody] Filter data)
        {
            try
            {
                var r = await _svc.GetAllAsync(data);
                return Ok(new { rs = r.Any() ? "ok" : "fail", data = r });
            }
            catch (Exception ex)
            {
                ex.LogEx();
                HandleBrokenCircuitException();
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [DisplayName("Borrado de Datos")]
        public async Task<IActionResult> Delete([FromBody] Filter data)
        {
            try
            {
                var idAcc = int.Parse(data.parameters["id"].ToString());
                var r = await _svc.GetItem(idAcc);
                if (r.IdRecord == 0)
                    return Ok(new { rs = "falla", msg = "" });

                var y = await _svc.Delete(r);
                return Ok(new { rs = y ? "ok" : "falla", msg = "" });
            }
            catch (Exception ex)
            {
                ex.LogEx();
                HandleBrokenCircuitException();
                return Ok(new { msg = "Hubo un error al intentar borrar..." });
            }
        }

        [HttpPost]
        [DisplayName("Guardado/Edicion")]
        public async Task<IActionResult> Save([FromForm] Recordatorio model)
        {
            try
            {
                var r = new Recordatorio();
                var r2 = new Mensaje();

                if (!Request.Form.TryGetValue("msg", out var msgData))
                    return View(nameof(Crud));

                if (!Request.Form.TryGetValue("grps", out var groups))
                    return View(nameof(Crud));

                if (string.IsNullOrEmpty(msgData) || string.IsNullOrEmpty(groups))
                    return View(nameof(Crud));

                if (model.IdRecord == 0)
                {
                    r2 = await _svcMsg.Add(new Mensaje()
                    {
                        DesMsg = msgData.ToString(),
                    });

                    if (r2.IdMsg == 0)
                        return View(nameof(Crud));

                    model.IdMsg = r2.IdMsg;
                    model.IdGrps = groups.ToString();
                    r = await _svc.Add(model);
                }
                else
                {
                    if (!model.IdMsg.HasValue)
                        return View(nameof(Crud));

                    var msgRow = await _svcMsg.GetItem(model.IdMsg.Value);
                    if (msgRow.IdMsg == 0)
                        return View(nameof(Crud));

                    msgRow.DesMsg = msgData.ToString();
                    r2 = await _svcMsg.Update(msgRow);
                    if (r2.IdMsg == 0)
                        return View(nameof(Crud));

                    model.IdGrps = groups.ToString();
                    r = await _svc.Update(model);
                }

                if (r.IdRecord > 0)
                    return RedirectToAction("Index");
            }
            catch (BrokenCircuitException ex)
            {
                HandleBrokenCircuitException();
                ex.LogEx();
            }
            catch (Exception ex)
            {
                ex.LogEx();
            }

            return Ok(nameof(Crud));
        }

        private void HandleBrokenCircuitException()
        {
            ViewBag.InoperativeMsg = "Service is inoperative, please try later on. (Business Msg Due to Circuit-Breaker)";
        }
    }
}
