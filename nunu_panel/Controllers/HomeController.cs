using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using nunu_panel.Models;
using RestSharp;

namespace nunu_panel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private static readonly string apiBaseUrl = "https://api-nunu.igrtec.net/api/";

        public async Task<IActionResult> Index()
        {
            var users = GetUsuarios();
            int totalUsuarios = 0;
            if (users != null)
            {
                totalUsuarios = users.Count;
            }
            ViewBag.TotalUsuarios = totalUsuarios;
            var anuncios = await GetAnuncios();
            int totalAnuncios = anuncios?.Count ?? 0;
            ViewBag.TotalAnuncios = totalAnuncios;
            var proveedores = await GetProveedores();
            int totalproveedores = proveedores?.Count ?? 0;
            ViewBag.TotalProveedores = totalproveedores;
            var servicios = await GetServicios();
            int totalservicios = servicios?.Count ?? 0;
            ViewBag.TotalServicios = totalservicios;

            var adminName = HttpContext.Session.GetString("AdminName");
            ViewBag.AdminName = adminName;
            return View();
        }

        public async Task<List<ServicioProveedorModel>?> GetServicios()
        {
            try
            {
                var options = new RestSharp.RestClientOptions(apiBaseUrl) { MaxTimeout = -1 };

                var client = new RestSharp.RestClient(options);
                var request = new RestSharp.RestRequest("/Servicios", Method.Get);
                request.AddHeader("Content-Type", "application/json");

                // Ejecutar la solicitud
                var response = await client.ExecuteAsync<List<ServicioProveedorModel>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string? content = response.Content;
                    List<ServicioProveedorModel>? servicios = JsonConvert.DeserializeObject<
                        List<ServicioProveedorModel>
                    >(content);
                    return servicios;
                }
                else
                {
                    // Manejo de errores o respuesta no exitosa
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<List<ProveedorModel>?> GetProveedores()
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
                    List<ProveedorModel>? Proveedores = JsonConvert.DeserializeObject<
                        List<ProveedorModel>
                    >(content);
                    return Proveedores;
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

        public async Task<List<AnuncioDetalleModel>?> GetAnuncios()
        {
            try
            {
                var options = new RestSharp.RestClientOptions(apiBaseUrl) { MaxTimeout = -1 };

                var client = new RestSharp.RestClient(options);
                var request = new RestSharp.RestRequest("/Anuncios", Method.Get);
                var response = client.Execute<List<AnuncioDetalleModel>>(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string? content = response.Content;
                    List<AnuncioDetalleModel>? anuncios = JsonConvert.DeserializeObject<
                        List<AnuncioDetalleModel>
                    >(content);
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
