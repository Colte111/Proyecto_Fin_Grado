using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;


using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    //2.- AÑADIR LA AUTHORIZACION --> si no hay usuario logeado, no se podra acceder a la web
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Ventas()
        {
            return View();
        }

        
        public IActionResult Compras()
        {
            return View();
        }

        public IActionResult Clientes()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}