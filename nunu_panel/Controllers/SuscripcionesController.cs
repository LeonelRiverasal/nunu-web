using Microsoft.AspNetCore.Mvc;
using nunu_panel.Models;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;

namespace nunu_panel.Controllers
{
    public class SuscripcionesController : Controller
    {
        private readonly ILogger<SuscripcionesController> _logger;

        public SuscripcionesController(ILogger<SuscripcionesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var adminName = HttpContext.Session.GetString("AdminName");    
            ViewBag.AdminName = adminName;
            var sus = GetSuscripciones();
            return View(sus);
        }
        public IActionResult Historial()
        {
            return View();
        }

        private static readonly string apiBaseUrl = "https://api-nunu.igrtecapi.site/api/";
public List<SuscripcionesModel>? GetSuscripciones()
        {
            try
            {
                var options = new RestSharp.RestClientOptions(apiBaseUrl)
                {
                    MaxTimeout = -1
                };

                var client = new RestSharp.RestClient(options);
                var request = new RestSharp.RestRequest("/Proveedores", Method.Get);
                var response = client.Execute<List<SuscripcionesModel>>(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string? content = response.Content;
                    List<SuscripcionesModel>? sus = JsonConvert.DeserializeObject<List<SuscripcionesModel>>(content);
                    return sus;
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
        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<bool> QuitarSub(int idProveedor)
        {
    
            try
            {
                var suscripcionModel = new SuscripcionesModel
            {
                suscripcion = false
            };
            var options = new RestSharp.RestClientOptions(apiBaseUrl)
                {
                    MaxTimeout = -1
                };
                var client = new RestSharp.RestClient(options);
                var request = new RestSharp.RestRequest($"/Proveedores/{idProveedor}", Method.Put);
                request.AddParameter("suscripcion", suscripcionModel.suscripcion);
                var response = await client.ExecuteAsync(request);                       
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
