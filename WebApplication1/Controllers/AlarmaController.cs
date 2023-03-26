using CapaDominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modelos;
using System.Threading;
using System.Threading.Tasks;

namespace carmaps.Controllers
{
    public class AlarmaController : Controller
    {
        private static Timer timer;
        CD_Alarma cd_alarma = new CD_Alarma();
        
        
        public IActionResult Index()
        {
            int id = (int)HttpContext.Session.GetInt32("id");
            List<ALARMA> _data = new List<ALARMA>();

            _data=cd_alarma._obtenerAlarma(id); 

            return View(_data);
        }

        [HttpPost]
        public IActionResult Index(ALARMA newAlarm)
        {
            int id = (int)HttpContext.Session.GetInt32("id");

            

            return View();
        }
    }
}
