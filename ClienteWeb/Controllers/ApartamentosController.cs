using AutoMapper;
using ClienteWeb.Models.ViewModels;
using ClienteWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ClienteWeb.Controllers
{
    public class ApartamentosController : Controller
    {
        private readonly ApiService _apiService;
        private readonly IMapper _mapper;

        public ApartamentosController(ApiService apiService, IMapper mapper)
        {
            _apiService = apiService;
            _mapper = mapper;   
        }

        //Listar Apartamentos
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> IndexAll()
        {
            if (!Request.Cookies.TryGetValue("UsuarioResponsable", out string usuario))
            {
                return RedirectToAction("Login", "Auth");
            }
            var apartamentosDTO = await _apiService.GetApartamentosAsync(); // Obtiene los DTOs
            if (apartamentosDTO == null)
            {
                return RedirectToAction("Logout", "Auth");
            }
            var apartamentosVM = _mapper.Map<IEnumerable<ApartamentoViewModel>>(apartamentosDTO); // Mapea a ViewModel
            return View(apartamentosVM);
        }
        //Listar Apto por usuario
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
                Response.Cookies.Delete("UsuarioResponsable");
                TempData["Error"] = "Tu cuenta ha sido eliminada. Inicia sesión nuevamente.";
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
