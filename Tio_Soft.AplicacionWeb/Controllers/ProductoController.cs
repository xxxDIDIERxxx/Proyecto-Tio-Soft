using Microsoft.AspNetCore.Mvc;

namespace Tio_Soft.AplicacionWeb.Controllers
{
    public class ProductoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
