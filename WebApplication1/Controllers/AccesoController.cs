using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using CapaDominio;

//1.- REFERENCES AUTHENTICATION COOKIE
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Modelos;

namespace WebApplication1.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //USAR REFERENCIAS Models y Data
        [HttpPost]
        public async Task<IActionResult> Index(USUARIO _usuario)
        {
            CD_Usuario cd_usuario = new CD_Usuario();

            var usuario = cd_usuario.ValidarUsuario(_usuario.Correo,_usuario.Constraseña);

            if (usuario != null)
            {

                //2.- CONFIGURACION DE LA AUTENTICACION
                #region AUTENTICACTION
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim("Correo", usuario.Correo),
                };
                
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                #endregion

                HttpContext.Session.SetString("username", usuario.Nombre);
                HttpContext.Session.SetInt32("id", usuario.USUARIOid);

                return RedirectToAction("Index", "Home");
            }
            else {
                TempData["error"] = "Login incorrecto";
                return View();
            }
            
        }

        public async Task<IActionResult> Salir()
        {
            //3.- CONFIGURACION DE LA AUTENTICACION
            #region AUTENTICACTION
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            #endregion

            return RedirectToAction("Index");

        }

    }
}
