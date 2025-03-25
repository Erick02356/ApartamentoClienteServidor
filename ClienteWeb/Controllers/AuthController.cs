using ClienteWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClienteWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiServices;
        public AuthController(ApiService apiService)
        {
            _apiServices = apiService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuarioResponsable)
        {
            var apartamento = await _apiServices.GetApartamentoPorUsuario(usuarioResponsable);
            if (apartamento != null)
            {
                Response.Cookies.Append("UsuarioResponsable", usuarioResponsable, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(3),
                    HttpOnly = true
                });
                return RedirectToAction("Index", "Apartamentos");
            }
            ViewBag.Error = "Usuario no encontrado";
            return View();
        }
    public IActionResult Logout()
    {
        Response.Cookies.Delete("UsuarioResponsable");
        return RedirectToAction("Login");
    }

    }
}
