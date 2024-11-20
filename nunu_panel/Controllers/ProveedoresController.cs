using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using nunu_panel.Models;
using RestSharp;

namespace nunu_panel.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly ILogger<ProveedoresController> _logger;
        private static readonly string apiBaseUrl = "https://api-nunu.igrtec.net/api/";

        public ProveedoresController(ILogger<ProveedoresController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var adminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminName = adminName;
            var proveedores = GetProveedores();
            return View(proveedores);
        }

        [HttpDelete]
        public IActionResult EliminarProveedor(int idProveedor)
        {
            var options = new RestClientOptions(apiBaseUrl) { MaxTimeout = -1 };

            var client = new RestClient(options);
            var request = new RestRequest($"/Proveedores/{idProveedor}", Method.Delete);
            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                string? content = response.Content;
                TempData["SuccessMensajeUsuario"] =
                    content != null ? $"Proveedor {idProveedor} eliminado con éxito!" : null;
                return Json(new { success = true, message = TempData["SuccessMensajeUsuario"] });
            }
            else
            {
                string content = response.Content;
                _logger.LogError(content);
                TempData["ErrorMensajeUsuario"] =
                    $"Error al eliminar el proveedor desde la API. {response.StatusCode}";
                return Json(new { success = false, message = TempData["ErrorMensajeUsuario"] });
            }
        }

        [HttpPost]
        public IActionResult AprovarProveedor(int idProveedor)
        {
            var options = new RestClientOptions(apiBaseUrl) { MaxTimeout = -1 };

            var client = new RestClient(options);
            var request = new RestRequest(
                $"/Proveedores/VerificarProveedor/{idProveedor}",
                Method.Post
            );
            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                //verificar documentacion con strapi
                var strapiINE = new RestRequest(
                    $"/Proveedores/VerificarDocumentacionProveedor/{idProveedor}",
                    Method.Post
                );
                var strapiINEResponse = client.Execute(strapiINE);
                if (strapiINEResponse.StatusCode == HttpStatusCode.OK)
                {
                    return Json(
                        new
                        {
                            success = true,
                            message = $"Proveedor {idProveedor} Aprobado con éxito"
                        }
                    );
                }
                else
                {
                    return Json(
                        new
                        {
                            success = false,
                            message = "Error al aprobar el proveedor:{0}",
                            strapiINEResponse.Content
                        }
                    );
                }
            }
            else
            {
                var error = JsonConvert.DeserializeObject(response.Content);

                _logger.LogError(error?.ToString());

                return Json(
                    new
                    {
                        success = false,
                        message = $"Error al aprobar el proveedor desde la API. {error?.ToString()}"
                    }
                );
            }
        }

        public List<ProveedorModel>? GetProveedores()
        {
            try
            {
                var options = new RestSharp.RestClientOptions(apiBaseUrl) { MaxTimeout = -1 };

                var client = new RestSharp.RestClient(options);
                var request = new RestSharp.RestRequest("/Proveedores", Method.Get);
                var response = client.Execute<List<ProveedorModel>>(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string? content = response.Content;
                    List<ProveedorModel>? proveedores = JsonConvert.DeserializeObject<
                        List<ProveedorModel>
                    >(content);
                    return proveedores;
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
