using Microsoft.AspNetCore.Mvc;
using nunu_panel.Models;
using System.Diagnostics;

namespace nunu_panel.Controllers
{
    public class PagosController : Controller
    {
        private readonly ILogger<PagosController> _logger;

        public PagosController(ILogger<PagosController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Historial()
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
