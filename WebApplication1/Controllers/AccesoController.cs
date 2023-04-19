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
        CD_Alarma cd_alarma = new CD_Alarma();
        public IActionResult Index()
        {
            string error = HttpContext.Request.Query["Error"];

            if (!string.IsNullOrEmpty(error))
            {
                // Muestra el mensaje de error al usuario
                ViewData["Error"] = error;
            }

            return View();
        }

        //USAR REFERENCIAS Models y Data
        [HttpPost]
        public async Task<IActionResult> Index(USUARIO _usuario)
        {
            CD_Usuario cd_usuario = new CD_Usuario();
            ALARMA _alarma = new ALARMA();

            string error = HttpContext.Request.Query["Error"];

            if (!string.IsNullOrEmpty(error))
            {
                // Muestra el mensaje de error al usuario
                ViewData["Error"] = error;
            }

            var usuario = cd_usuario.ValidarUsuario(_usuario.Correo,_usuario.Constraseña);

            if (usuario != null)
            {

                //2.- CONFIGURACION DE LA AUTENTICACION
                #region AUTENTICACTION
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim("Correo", usuario.Correo),
                    new Claim("id",usuario.USUARIOid.ToString())
                };

              
                
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                #endregion

                //Guardo el id en una sesion
                
                HttpContext.Session.SetInt32("id", usuario.USUARIOid);

                

                return RedirectToAction("Index", "Auto");
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
