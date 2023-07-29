using Microsoft.AspNetCore.Mvc;

namespace Tio_Soft.AplicacionWeb.Controllers
{
    public class VentaController : Controller
    {
        public IActionResult NuevaVenta()
        {
            return View();
        }       
        public IActionResult HistorialVenta()
        {
            return View();
        }
    }
}
