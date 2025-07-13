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
    public class TamanoController : Controller
    {
        private readonly ITamanoService _svc;

        public TamanoController(ITamanoService TamanoService)
        {
            _svc = TamanoService ?? throw new ArgumentNullException(nameof(TamanoService));
        }

        [DisplayName("Pantalla de Inicio")]
        public IActionResult Index()
        {
            return View();
        }

        [DisplayName("Pantalla de Inicio")]
        public async Task<IActionResult> Crud(int id)
        {
            return View(id > 0 ? await _svc.GetItem(id) : new Tamano());
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
                if (r.IdTam == 0)
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
        public async Task<IActionResult> Save([FromForm] Tamano model)
        {
            try
            {
                var r = new Tamano();
                if (model.IdTam == 0)
                {
                    r = await _svc.Add(model);
                }
                else
                {
                    r = await _svc.Update(model);
                }

                if (r.IdTam > 0)
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
