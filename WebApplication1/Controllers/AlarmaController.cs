using CapaDominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Modelos;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using WebApplication1;

namespace carmaps.Controllers
{
    [Authorize]

    public class AlarmaController : Controller
    {
        public CD_Alarma cd_alarma = new CD_Alarma();
        public USUARIO user = new USUARIO();
        public AutoController autoController = new AutoController();
        public IHubContext<AlarmHub> _alarmHubContext;
        

        #region FUNCIONES_ALARMA

        //Funcion para obtener el ID del usuario almacenado en el HttpContext
        public int ID()
        {
            var user = HttpContext.User;
            var _id = user.FindFirstValue("id");

            int id = Int32.Parse(_id);

            return id;
        }

        //Funcion para obtener la fecha de la alarma **Siempre la fecha mas cercana a la actual**
        public DateTime ALARMA()
        {
            var _alarma = new ALARMA();
            _alarma = cd_alarma._primeraAlarma(ID());

            return _alarma.Fecha;
        }
        
        #endregion

        #region FUNCIONES_INICIAR_RUTA

        #endregion

        public IActionResult Index()
        {
            
            List<ALARMA> _data = new List<ALARMA>();

            _data=cd_alarma._obtenerAlarma(ID()); 

            return View(_data);
        }
        public IActionResult NewAlarma()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewAlarma(ALARMA newAlarm)
        {
            var cts = new CancellationTokenSource();

            try
            {
                if(newAlarm.Fecha < ALARMA())
                {
                    cd_alarma._insertarAlarma(ID(), newAlarm.Fecha);
                    autoController.StartAlarmAsync(newAlarm.Fecha);
                }
                else
                {
                    cts.Cancel();
                    cd_alarma._insertarAlarma(ID(), newAlarm.Fecha);
                    ViewBag.successAlarma = "Alarma Programada";
                }

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
