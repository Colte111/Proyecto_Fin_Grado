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
        CD_Alarma cd_alarma = new CD_Alarma();
        USUARIO user = new USUARIO();
        public IHubContext<AlarmHub> _alarmHubContext;
        
        

        #region FUNCIONES_ALARMA
        public int ID()
        {
            var user = HttpContext.User;
            var _id = user.FindFirstValue("id");

            int id = Int32.Parse(_id);

            return id;
        }
        public DateTime ALARMA()
        {
            var _alarma = new ALARMA();
            _alarma = cd_alarma._primeraAlarma(ID());

            return _alarma.fecha;
        }
        public async Task StartAlarmAsync(DateTime alarmTime,CancellationTokenSource cts)
        {
            //Compruebo que el valor del DateTime es mayor a now **EXPLICAR**
            if (alarmTime > DateTime.Now)
            {
                // Obtener la hora actual y calcular la cantidad de segundos hasta que suene la alarma
                var currentTime = DateTime.Now;
                var timeToAlarm = (int)(alarmTime - currentTime).TotalSeconds;

                // Esperar hasta que sea hora de sonar la alarma
                await Task.Delay(timeToAlarm * 1000);

                // Enviar el mensaje a través del concentrador de SignalR
                await _alarmHubContext.Clients.All.SendAsync("alarm");
            }
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
        public async Task<IActionResult> NewAlarma(ALARMA newAlarm)
        {
            var cts = new CancellationTokenSource();

            try
            {
                cts.Cancel();
                cd_alarma._insertarAlarma(ID(), newAlarm.fecha);
                await StartAlarmAsync(ALARMA(),cts);
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
