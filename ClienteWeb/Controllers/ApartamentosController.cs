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
        public async Task<IActionResult> Index()
        {
            var apartamentos = await _apiService.GetApartamentosAsync();
            return View(apartamentos);
        }

        //Creación
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApartamentoViewModel model)
        {

            if (ModelState.IsValid &&  await _apiService.CreateApartamentoAsync(model))
                return RedirectToAction("Index");
                

            return View(model);
        }

        //Edit
        public async Task<IActionResult> Edit (int id)
        {
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
            var response = await _apiService.DeleteApartamentoAsync(id);

            if (!response)
            {
                TempData["ErrorMessage"] = "No se pudo eliminar el apartamento.";
            }

            return RedirectToAction("Index"); // Redirigir sin mostrar pantalla de confirmación
        }
    }
}
