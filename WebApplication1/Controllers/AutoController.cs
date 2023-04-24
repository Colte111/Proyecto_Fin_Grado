using CapaDominio;
using capaDominio;
using Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CapaAccesoBBDD;
using Microsoft.AspNetCore.SignalR;
using WebApplication1;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace carmaps.Controllers
{
    [Authorize]
    public class AutoController : Controller
    {
        CD_Alarma cd_alarma = new CD_Alarma();
        private CD_Automovil CD_Auto = new CD_Automovil();
        public IHubContext<AlarmHub> _alarmHubContext;

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

            return _alarma.Fecha;
        }
        public async Task StartAlarmAsync(DateTime alarmTime)
        {
            //Compruebo que el valor del DateTime es mayor a now **EXPLICAR**
            if(alarmTime > DateTime.Now)
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

        public ActionResult Index()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            StartAlarmAsync(ALARMA());
            List<AUTOMOVIL> _coche = new List<AUTOMOVIL>();
            _coche = CD_Auto.MostrarCOCHE(ID());
            return View((_coche));
        }

        [HttpPost]
        public JsonResult GetAuthToken(string lati, string longi)
        {
            //Obtengo la ubicacion desde js y la guardo en la session para guardarlas en la bbdd posteriormente
            HttpContext.Session.SetString("lati", lati);
            HttpContext.Session.SetString("longi", longi);

            //Si todo ha ido bien,saldra un alert notificandolo
            return Json(new { Msg = "Ubicacion actualizada!" });
        }

        [HttpPost]
        public ActionResult Index(int idAUTO)
        {
            //https://www.google.com/maps/dir/'@latiActual,-@longiActual'/''/@latiCoche,-@longiCoche,11z/data=!3m1!4b1!4m14!4m13!1m5!1m1!1s0x0:0xc8c36b17074ece53!2m2!1d-0.5068824!2d38.5469467!1m5!1m1!1s0xd623662d742628d:0x402af6ed721d5c0!2m2!1d-0.4909626!2d38.3460627!3e0
            string lati = (string)HttpContext.Session.GetString("lati");
            string longi = (string)HttpContext.Session.GetString("longi");

            try
            {
                //Llamo a la funcion para guardar la ubicacion en la BBDD
                CD_Auto.updatecoords(lati, longi, ID(), idAUTO);
            }
            catch
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public ActionResult NewAuto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewAuto(AUTOMOVIL automovil)
        {
            try
            {
                CD_Auto.InsertarCOCHE(automovil.Matricula, automovil.Marca,ID());
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult DeleteAuto()
        {
            List<AUTOMOVIL> _coche = new List<AUTOMOVIL>();

            //Funcion para mostrar los vehiculos del usuario
            _coche = CD_Auto.MostrarCOCHE(ID());

            return View(_coche);
        }

        [HttpPost]
        public ActionResult DeleteAuto(AUTOMOVIL modelo)
        {
            int AUTOid = modelo.AUTOMOVILid;
            try
            {
                CD_Auto.EliminarCOCHE(AUTOid);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}
