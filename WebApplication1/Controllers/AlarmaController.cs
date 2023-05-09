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
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace carmaps.Controllers
{
    [Authorize]

    public class AlarmaController : Controller
    {
        public CD_Alarma cd_alarma = new CD_Alarma();
        public USUARIO user = new USUARIO();
        public IHubContext<AlarmHub> _alarmHubContext;
        public CancellationTokenSource cts = new CancellationTokenSource();

        public AlarmaController(IHubContext<AlarmHub> alarmHubContext)
        {
            _alarmHubContext = alarmHubContext;
        }
        

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
        public async Task StartAlarmAsync(DateTime alarmTime, CancellationToken cancellationToken)
        {
            //Comparo con datetime.now porque aunque no haya recogido ninguna fecha de la BBDD, un DateTime nunca puede ser null, entonces siempre devolvera un valor de fecha 01/01/0001
            if (alarmTime > DateTime.Now)
            {
                if (!cancellationToken.IsCancellationRequested)
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
        }

        #endregion

        #region FUNCIONES_INICIAR_RUTA

        #endregion

        public IActionResult Index(ALARMA auto)
        {
            List<ALARMA> _data = new List<ALARMA>();
            int ALARMAid = 0;

            _data=cd_alarma._obtenerListAlarma(auto.AUTOMOVILid);

            foreach(var c in _data)
            {
                ALARMAid = c.ALARMAid;
            }

            HttpContext.Session.SetInt32("AUTOMOVILid", auto.AUTOMOVILid);
            HttpContext.Session.SetInt32("ALARMAid", ALARMAid);

            #region COMENTARIOS de ERROR o EXITO
            if (TempData.ContainsKey("AccesoNewAlarm"))
            {
                ViewBag.AccesoNewAlarm = TempData["AccesoNewAlarm"].ToString();
            }
            if (TempData.ContainsKey("SuccessActualizar"))
            {
                ViewBag.SuccessActualizar = TempData["SuccessActualizar"].ToString();
            }
            #endregion

            return View(_data);
        }
        
        public IActionResult NewAlarma()
        {
            List <ALARMA> alarma = new List<ALARMA>();
            int AUTOMOVILid = (int)HttpContext.Session.GetInt32("AUTOMOVILid");

            alarma = cd_alarma._obtenerListAlarma(AUTOMOVILid);
            
            //Como solo se puede tener una alarma por coche,compruebo que no hay
            //ninguna para dejarle entrar en la vista
            if(alarma.Count > 0)
            {
                TempData["AccesoNewAlarm"] = "Solo puedes programar una alarma por vehiculo";
                return RedirectToAction
                (
                    "Index",
                    new { AUTOMOVILid = (int)HttpContext.Session.GetInt32("AUTOMOVILid") }
                );
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        public IActionResult NewAlarma(ALARMA newAlarm)
        {
            int AUTOMOVILid = (int)HttpContext.Session.GetInt32("AUTOMOVILid");
            try
            {
                if(newAlarm.Fecha < ALARMA())
                {
                    cts.Cancel();
                    cd_alarma._insertarAlarma(AUTOMOVILid, newAlarm.Fecha);
                    StartAlarmAsync(newAlarm.Fecha,cts.Token);
                }
                else
                {
                    cd_alarma._insertarAlarma(AUTOMOVILid, newAlarm.Fecha);
                }
                ViewBag.successAlarma = "Alarma Programada";
                return View();
            }
            catch 
            {
                ViewBag.errorAlarma = "Error, pruebe de nuevo";
                return View();
            }           
        }

        [HttpPost]
        public IActionResult DeleteAlarma(int ALARMAid) 
        {
            ALARMA alarma = new ALARMA();
            try
            {
                cd_alarma._eliminarAlarma(ALARMAid);
                TempData["SuccessBorrar"] = "Alarma eliminada";
                return RedirectToAction
                (
                    "Index",
                    new { AUTOMOVILid = (int)HttpContext.Session.GetInt32("AUTOMOVILid") }
                );
            }
            catch
            {
                TempData["ErrorBorrar"] = "Error al eliminar";
                return RedirectToAction("Index");
            }
        }

        
        public IActionResult UpdateAlarma()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateAlarma(ALARMA alarma)
        {
            int ALARMAid = (int)HttpContext.Session.GetInt32("ALARMAid");
            try
            {
                cd_alarma._actualizarAlarma(ALARMAid, alarma.Fecha);
                TempData["SuccessActualizar"] = "Alarma Actualizada";
                return RedirectToAction
                (
                    "Index",
                    new { AUTOMOVILid = (int)HttpContext.Session.GetInt32("AUTOMOVILid") }
                );
            }
            catch
            {
                ViewBag.errorActualizar = "Error al actualizar";
                return View();
            }
            
        }

    }
}
