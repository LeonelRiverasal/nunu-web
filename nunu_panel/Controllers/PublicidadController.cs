using Microsoft.AspNetCore.Mvc;
using nunu_panel.Models;
using System.Diagnostics;

namespace nunu_panel.Controllers
{
    public class PublicidadController : Controller
    {
        private readonly ILogger<PublicidadController> _logger;

        public PublicidadController(ILogger<PublicidadController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PublicidadDetalle()
        {
            return View();
        }

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
