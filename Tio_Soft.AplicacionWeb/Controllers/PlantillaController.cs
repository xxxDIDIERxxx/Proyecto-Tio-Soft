using Microsoft.AspNetCore.Mvc;

namespace Tio_Soft.AplicacionWeb.Controllers
{
    public class PlantillaController : Controller
    {
        public IActionResult EnviarClave(string correo, string clave)
        {
            ViewData["Correo"] = correo;//permite compartir informacion con nuestra vista
            ViewData["Clave"] = clave;
            ViewData["Url"] = $"{this.Request.Scheme}://{this.Request.Host}";

            return View();
        } 

        public IActionResult RestablecerClave(string clave)
        {
            ViewData["Clave"] = clave;

            return View();
        }
    }
}
