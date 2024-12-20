using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using nunu_panel.Models;
using RestSharp;

namespace nunu_panel.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private static readonly string apiBaseUrl = "https://api-nunu.igrtec.net/api/";

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string? error)
        {
            ViewBag.Error = error;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            var options = new RestClientOptions(apiBaseUrl) { MaxTimeout = -1 };
            var client = new RestClient(options);
            var request = new RestRequest("/Administradores/login", Method.Post);
            request.AddHeader("Content-Type", "application/json");

            var st = new { correo = loginModel.correo, contraseña = loginModel.contrasena };
            string jsn = JsonConvert.SerializeObject(st);
            request.AddJsonBody(jsn, ContentType.Json);
            var response = client.Execute(request);

            if (
                response.StatusCode == HttpStatusCode.Created
                || response.StatusCode == HttpStatusCode.OK
            )
            {
                var admin = JsonConvert.DeserializeObject<AdminResponse>(response.Content);

                HttpContext.Session.SetInt32("AdminId", admin.id_admin);
                HttpContext.Session.SetString("AdminName", admin.nombre);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos");
                return RedirectToAction(
                    nameof(Index),
                    new { error = "Correo o contraseña incorrectos" }
                );
            }
        }

        public class TokenResponse
        {
            public string AccessToken { get; set; }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }
    }
}
