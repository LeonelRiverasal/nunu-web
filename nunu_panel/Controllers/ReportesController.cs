using Microsoft.AspNetCore.Mvc;
using nunu_panel.Models;
using System.Diagnostics;

namespace nunu_panel.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ILogger<ReportesController> _logger;

        public ReportesController(ILogger<ReportesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
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
