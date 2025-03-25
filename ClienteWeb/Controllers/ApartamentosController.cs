using ClienteWeb.Models.ViewModels;
using ClienteWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ClienteWeb.Controllers
{
    public class ApartamentosController : Controller
    {
        private readonly ApiService _apiService;

        public ApartamentosController(ApiService apiService)
        {
            _apiService = apiService;
        }

        //Listar Apartamentos
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> IndexAll()
        {
            if (!Request.Cookies.TryGetValue("UsuarioResponsable", out string usuario))
            {
                return RedirectToAction("Login", "Auth");
            }
            var apartamento = await _apiService.GetApartamentosAsync();
            if (apartamento == null)
            {
                return RedirectToAction("Logout", "Auth");
            }
            return View(apartamento);
        }
        //Listar Apto
        public async Task<IActionResult> Index()
        {
            var usuarioResponsable = Request.Cookies["UsuarioResponsable"];
            if (string.IsNullOrEmpty(usuarioResponsable))
            {
                return RedirectToAction("Login", "Auth");
            }

            var apartamento = await _apiService.GetApartamentoPorUsuario(usuarioResponsable);
            if (apartamento == null)
            {
                TempData["Error"] = "No tienes apartamento asignado.";
                return RedirectToAction("Login", "Auth");
            }

            return View(apartamento);
        }
        //Creación
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApartamentoViewModel model)
        {
            if (!Request.Cookies.TryGetValue("UsuarioResponsable", out string usuario))
            {
                return RedirectToAction("Login", "Auth");
            }
            if (ModelState.IsValid &&  await _apiService.CreateApartamentoAsync(model))
                return RedirectToAction("Index");
            return View(model);
        }

        //Edit
        public async Task<IActionResult> Edit (int id)
        {
            if (!Request.Cookies.TryGetValue("UsuarioResponsable", out string usuario))
            {
                return RedirectToAction("Login", "Auth");
            }
            var apartamento = await _apiService.GetApartamentoByIdAsync(id);
            return apartamento == null ? NotFound() : View(apartamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ApartamentoViewModel model)
        {
            if (ModelState.IsValid && await _apiService.UpdateApartamentoAsync(id, model))
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!Request.Cookies.TryGetValue("UsuarioResponsable", out string usuario))
            {
                return RedirectToAction("Login", "Auth");
            }
            var response = await _apiService.DeleteApartamentoAsync(id);

            if (!response)
            {
                TempData["ErrorMessage"] = "No se pudo eliminar el apartamento.";
            }

            return RedirectToAction("Index"); // Redirigir sin mostrar pantalla de confirmación
        }
    }
}
