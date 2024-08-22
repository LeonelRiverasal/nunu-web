using Microsoft.AspNetCore.Mvc;
using nunu_panel.Models;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;


namespace nunu_panel.Controllers
{
    public class AnunciosController : Controller
    {
        private readonly ILogger<AnunciosController> _logger;

        public AnunciosController(ILogger<AnunciosController> logger)
        {
            _logger = logger;
        }

        private static readonly string apiBaseUrl = "https://api-nunu.igrtecapi.site/api/";

        public IActionResult Index()
        {
            var adminName = HttpContext.Session.GetString("AdminName");    
            ViewBag.AdminName = adminName;
            var anuncios = GetAnuncios();
            return View(anuncios);

        }
        [HttpPost]
        public IActionResult RegistrarAnuncio(AnuncioModel anuncio)
        {
            var options = new RestClientOptions(apiBaseUrl)
            {
                MaxTimeout = -1
            };

            var client = new RestClient(options);
            var request = new RestRequest("/Anuncios", Method.Post);
           /* var anunciovrd = new AnuncioModel();
            anunciovrd.nombre_empresa_anunciante = anuncio.nombre_empresa_anunciante;
            anunciovrd.tipo_Oferta = anuncio.tipo_Oferta;
            anunciovrd.descripcion_oferta = anuncio.descripcion_oferta;
            anunciovrd.direccion = anuncio.direccion;
            anunciovrd.telefono = anuncio.telefono;
            anunciovrd.vigencia_oferta = anuncio.vigencia_oferta;*/
             request.AddParameter("Nombre_empresa_anunciante", anuncio.nombre_empresa_anunciante);
            request.AddParameter("Tipo_oferta", anuncio.tipo_Oferta);
            request.AddParameter("Descripcion_oferta", anuncio.descripcion_oferta);
            request.AddParameter("Direccion", anuncio.direccion);
            request.AddParameter("Telefono", anuncio.telefono);
            request.AddParameter("Vigencia_oferta", anuncio.vigencia_oferta.ToString("yyyy-MM-dd"));

            if (anuncio.Imagen_anuncio != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    anuncio.Imagen_anuncio.CopyTo(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();

                    request.AddFile("Imagen_anuncio", fileBytes, anuncio.Imagen_anuncio.FileName);
                }
            }

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
            {
                return Json(new { success = true, message = "Anuncio registrado con éxito!" });
            }
            else
            {                
                return Json(new { success = false, message = $"Error al registrar el anuncio en la API. {response.StatusCode}" });
            }
            
        }

        [HttpDelete]
        public IActionResult EliminarAnuncio(int idAnuncio)
        {
            var options = new RestClientOptions(apiBaseUrl)
            {
                MaxTimeout = -1
            };

            var client = new RestClient(options);
            var request = new RestRequest($"/Anuncios/{idAnuncio}", Method.Delete);
            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                string? content = response.Content;
                TempData["SuccessMensajeUsuario"] = content != null ? $"anuncio {idAnuncio} eliminado con éxito!" : null;
                return Json(new { success = true, message = TempData["SuccessMensajeUsuario"] });
            }
            else
            {
                TempData["ErrorMensajeUsuario"] = $"Error al eliminar el anuncio desde la API. {response.StatusCode}";
                return Json(new { success = false, message = TempData["ErrorMensajeUsuario"] });
            }
        }
        public async Task<bool> UpdateAnuncio(AnuncioDetalleModel anuncio)
        {
            try
            {
                var options = new RestSharp.RestClientOptions(apiBaseUrl)
                {
                    MaxTimeout = -1
                };

                var client = new RestSharp.RestClient(options);
                var request = new RestSharp.RestRequest($"/Anuncios/{anuncio.id_anuncio}", Method.Put);
                request.AddJsonBody(anuncio);

                var response = await client.ExecuteAsync(request);
                
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
        public AnuncioDetalleModel? GetAnuncioById(int id)
{
    try
    {
        var options = new RestSharp.RestClientOptions(apiBaseUrl)
        {
            MaxTimeout = -1
        };

        var client = new RestSharp.RestClient(options);
        var request = new RestSharp.RestRequest($"/Anuncios/{id}", Method.Get);
        var response = client.Execute<AnuncioDetalleModel>(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            string? content = response.Content;
            AnuncioDetalleModel? anuncio = JsonConvert.DeserializeObject<AnuncioDetalleModel>(content);
            return anuncio;
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

        public List<AnuncioDetalleModel>? GetAnuncios()
        {
            try
            {
                var options = new RestSharp.RestClientOptions(apiBaseUrl)
                {
                    MaxTimeout = -1
                };

                var client = new RestSharp.RestClient(options);
                var request = new RestSharp.RestRequest("/Anuncios", Method.Get);
                var response = client.Execute<List<AnuncioDetalleModel>>(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string? content = response.Content;
                    List<AnuncioDetalleModel>? anuncios = JsonConvert.DeserializeObject<List<AnuncioDetalleModel>>(content);
                    return anuncios;
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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
