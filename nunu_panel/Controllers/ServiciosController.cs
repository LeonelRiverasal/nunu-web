using Microsoft.AspNetCore.Mvc;
using nunu_panel.Models;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;

namespace nunu_panel.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly ILogger<ServiciosController> _logger;
        
        private static readonly string apiBaseUrl = "https://api-nunu.igrtecapi.site/api/";

        public ServiciosController(ILogger<ServiciosController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var adminName = HttpContext.Session.GetString("AdminName");    
            ViewBag.AdminName = adminName;
            var servicios = GetServicios();
            return View(servicios);
        }
        public List<ServicioProveedorModel>? GetServicios()
        {
        try
                    {
                        var options = new RestSharp.RestClientOptions(apiBaseUrl)
                        {
                            MaxTimeout = -1
                        };

                        var client = new RestSharp.RestClient(options);
                        var request = new RestSharp.RestRequest("/Servicios", Method.Get);
                        var response = client.Execute<List<ServicioProveedorModel>>(request);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            string? content = response.Content;
                            List<ServicioProveedorModel>? serv = JsonConvert.DeserializeObject<List<ServicioProveedorModel>>(content);
                            return serv;
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
          [HttpDelete]
        public IActionResult EliminarServicio(int idServicio)
        {
            var options = new RestClientOptions(apiBaseUrl)
            {
                MaxTimeout = -1
            };

            var client = new RestClient(options);
            var request = new RestRequest($"/Servicios/{idServicio}", Method.Delete);
            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                string? content = response.Content;
                TempData["SuccessMensajeUsuario"] = content != null ? $"Servicio {idServicio} eliminado con éxito!" : null;
                return Json(new { success = true, message = TempData["SuccessMensajeUsuario"] });
            }
            else
            {
                TempData["ErrorMensajeUsuario"] = $"Error al eliminar el servicio desde la API. {response.StatusCode}";
                return Json(new { success = false, message = TempData["ErrorMensajeUsuario"] });
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
