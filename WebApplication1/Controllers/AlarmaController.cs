using CapaDominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modelos;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace carmaps.Controllers
{
    [Authorize]
    public class AlarmaController : Controller
    {
        CD_Alarma cd_alarma = new CD_Alarma();
        USUARIO user = new USUARIO();

        public int ID()
        {
            var user = HttpContext.User;
            var _id = user.FindFirstValue("id");

            int id = Int32.Parse(_id);

            return id;
        }
        
        
        public IActionResult Index()
        {
            
            List<ALARMA> _data = new List<ALARMA>();

            _data=cd_alarma._obtenerAlarma(ID()); 

            return View(_data);
        }
        public IActionResult InsertarAlarma()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertarAlarma(ALARMA newAlarm)
        {
            try
            {
                cd_alarma._insertarAlarma(ID(), newAlarm.fecha);
                ViewBag.successAlarma = "Alarma Programada";

                return View();
            }
            catch 
            {
                ViewBag.errorAlarma = "Error, pruebe de nuevo";
                return View();
            }           
        }
    }
}
