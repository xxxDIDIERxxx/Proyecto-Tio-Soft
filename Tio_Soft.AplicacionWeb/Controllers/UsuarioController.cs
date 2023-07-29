using Microsoft.AspNetCore.Mvc;

namespace Tio_Soft.AplicacionWeb.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
