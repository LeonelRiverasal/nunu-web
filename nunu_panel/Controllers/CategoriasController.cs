using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using nunu_panel.Models;
using RestSharp;

namespace nunu_panel.Controllers;

public class CategoriasController : Controller
{
    private static readonly string apiBaseUrl = "https://api-nunu.igrtecapi.site/api/";

    private static readonly RestClientOptions options = new RestClientOptions(apiBaseUrl)
    {
        MaxTimeout = -1
    };

    private static readonly RestClient client = new RestClient(options);

    public async Task<IActionResult> Index()
    {
        var request = new RestRequest("/Categorias");
        var response = await client.ExecuteAsync(request);

        string? content = response.Content ?? "[]";
        var model = JsonConvert.DeserializeObject<List<CategoriaModel>>(content);

        model ??= new List<CategoriaModel>();

        return View(model);
    }

    public IActionResult Nueva()
    {
        return View(new CategoriaModel());
    }

    [HttpPost]
    public async Task<IActionResult> Nueva(CategoriaModel model)
    {
        if (ModelState.IsValid)
        {
            var request = new RestRequest("/Categorias", Method.Post);

            request.AddParameter("Categoria", model.categoria);

            if (model.Imagen != null)
            {
                using var ms = new MemoryStream();
                model.Imagen.CopyTo(ms);
                byte[] bytes = ms.ToArray();
                request.AddFile("Imagen_Categoria", bytes, model.Imagen.FileName);
            }

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            Console.WriteLine("Error creando la categoría: " + response.StatusDescription);

            ModelState.AddModelError(
                string.Empty,
                "No se pudo crear la nueva categoría de servicios, por favor, inténtelo más tarde."
            );
        }

        return View(model);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var request = new RestRequest($"/Categorias/{id}");
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Categoría no encontrada {id}: " + response.StatusDescription);

            return RedirectToAction("Index");
        }

        var content = response.Content ?? "[]";
        var model = JsonConvert.DeserializeObject<CategoriaModel>(content);

        model ??= new CategoriaModel();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(CategoriaModel model)
    {
        if (ModelState.IsValid)
        {
            int id = model.id_categoria;

            var request = new RestRequest($"/Categorias/{id}", Method.Put);

            request.AddParameter("categoria.Categoria", model.categoria);

            if (model.Imagen != null)
            {
                using var ms = new MemoryStream();
                model.Imagen.CopyTo(ms);
                byte[] bytes = ms.ToArray();
                request.AddFile("categoria.Imagen_Categoria", bytes, model.Imagen.FileName);
            }

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            Console.WriteLine("Error actualizando la categoría: " + response.StatusDescription);

            ModelState.AddModelError(
                string.Empty,
                "No se pudo actualizar la categoría de servicios, por favor, inténtelo más tarde."
            );
        }

        return View(model);
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        var request = new RestRequest($"/Categorias/{id}", Method.Delete);
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Categoría no eliminada {id}: " + response.StatusDescription);
        }

        return RedirectToAction("Index");
    }
}
