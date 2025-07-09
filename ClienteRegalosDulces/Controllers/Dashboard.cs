using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClienteRegalosDulces.Controllers
{
    public class Dashboard : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}
