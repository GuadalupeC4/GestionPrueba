using Microsoft.AspNetCore.Mvc;

namespace GestionPrueba.Controllers
{
    public class InicioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Bebidas()
        {
            return View("Bebidas");
        }
        public IActionResult Snack()
        {
            return View("Snack");
        }
        public IActionResult Regresar()
        {
            return View("Index");
        }
      
    }
}
