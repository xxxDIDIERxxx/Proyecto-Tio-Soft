using Microsoft.AspNetCore.Mvc;

namespace Tio_Soft.AplicacionWeb.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
