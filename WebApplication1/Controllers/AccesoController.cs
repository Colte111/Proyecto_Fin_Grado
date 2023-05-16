using Microsoft.AspNetCore.Mvc;
using CapaDominio;

//1.- REFERENCES AUTHENTICATION COOKIE
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Modelos;
using CapaAccesoBBDD;

namespace WebApplication1.Controllers
{
    public class AccesoController : Controller
    {
        
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
            AUTOMOVIL _alarma = new AUTOMOVIL();

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
                    new Claim("Nombre", usuario.Nombre),
                    new Claim("Correo", usuario.Correo),
                    new Claim("id",usuario.USUARIOid.ToString())
                };

              
                
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                #endregion
                

                return RedirectToAction("Index", "Auto");
            }
            else {
                TempData["error"] = "Login incorrecto";
                return View();
            }
            
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task <ActionResult> Register(USUARIO _usuario)
        {
            CD_Usuario usuario = new CD_Usuario();
            try
            {
                usuario.InsertarUsuario(_usuario.Nombre, _usuario.Apellidos, _usuario.Correo, _usuario.FechaNacimiento, _usuario.Genero, _usuario.Constraseña);
                var user = usuario.ValidarUsuario(_usuario.Correo, _usuario.Constraseña);

                if (user != null)
                {

                    //2.- CONFIGURACION DE LA AUTENTICACION
                    #region AUTENTICACTION
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Nombre),
                        new Claim("Correo", user.Correo),
                        new Claim("id",user.USUARIOid.ToString())
                    };



                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    #endregion

                    //Guardo el id en una sesion

                    HttpContext.Session.SetInt32("id", user.USUARIOid);



                    return RedirectToAction("Index", "Auto");
                }
                else
                {
                    TempData["error"] = "Login incorrecto";
                    return View();
                }
            }
            catch (Exception ex)
            {

                string error = ex.Message;
                return View();

            }
            return View();
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
