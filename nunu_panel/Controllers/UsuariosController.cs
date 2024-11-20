using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using nunu_panel.Models;
using RestSharp;

namespace nunu_panel.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ILogger<UsuariosController> logger)
        {
            _logger = logger;
        }

        private static readonly string apiBaseUrl = "https://api-nunu.igrtec.net/api/";

        public IActionResult Index()
        {
            var adminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminName = adminName;
            var usuarios = GetUsuarios();
            return View(usuarios);
        }

        [HttpDelete]
        public IActionResult EliminarUsuario(int idUsuario)
        {
            var options = new RestClientOptions(apiBaseUrl) { MaxTimeout = -1 };

            var client = new RestClient(options);
            var request = new RestRequest($"/Usuarios/{idUsuario}", Method.Delete);
            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                string? content = response.Content;
                TempData["SuccessMensajeUsuario"] =
                    content != null ? $"¡Usuario {idUsuario} eliminado con éxito!" : null;
                return Ok(new { success = true, message = TempData["SuccessMensajeUsuario"] });
            }

            TempData["ErrorMensajeUsuario"] =
                $"Error al eliminar el usuario desde la API. {response.StatusCode}";
            return StatusCode(
                (int)response.StatusCode,
                new { success = false, message = TempData["ErrorEliminarUsuario"] }
            );
        }

        public List<UsuarioModel>? GetUsuarios()
        {
            try
            {
                var options = new RestSharp.RestClientOptions(apiBaseUrl) { MaxTimeout = -1 };

                var client = new RestSharp.RestClient(options);
                var request = new RestSharp.RestRequest("/Usuarios", Method.Get);
                var response = client.Execute<List<UsuarioModel>>(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string? content = response.Content;
                    List<UsuarioModel>? usuarios = JsonConvert.DeserializeObject<
                        List<UsuarioModel>
                    >(content);
                    return usuarios;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
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
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }
    }
}
