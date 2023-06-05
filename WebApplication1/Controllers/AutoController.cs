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
        public CancellationTokenSource cts = new CancellationTokenSource();

        public AutoController(IHubContext<AlarmHub> alarmHubContext)
        {
            _alarmHubContext = alarmHubContext;
        }

        #region FUNCIONES ALARMA
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
        public int AUTOMOVILid_alarma()
        {
            var _alarma = new ALARMA();
            _alarma = cd_alarma._primeraAlarma(ID());

            return _alarma.AUTOMOVILid;
        }
        public async Task StartAlarmAsync(DateTime alarmTime,CancellationToken cancellationToken)
        {
            //Comparo con datetime.now porque aunque no haya recogido ninguna fecha de la BBDD
            // un DateTime nunca puede ser null, entonces siempre devolvera un valor de fecha 01/01/0001
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

        public ActionResult Index()
        {
            List<AUTOMOVIL> _coche = new List<AUTOMOVIL>();
            ALARMA _alarma = new ALARMA();

            _alarma = cd_alarma._primeraAlarma(ID());

            DateTime initialValue = DateTime.Parse("01/01/0001 0:00:00");

            if (ALARMA() != initialValue)
            {
                StartAlarmAsync(_alarma.Fecha, cts.Token);

                //Obtengo las coordenadas del coche con la alarma más próxima a la fecha actual y las guardo en una session
                AUTOMOVIL coordenadas = new AUTOMOVIL();
                coordenadas = CD_Auto._obtenerUbicacion(_alarma.AUTOMOVILid);
                HttpContext.Session.SetString("LatitudCoche", coordenadas.Latitud);
                HttpContext.Session.SetString("LongitudCoche", coordenadas.Longitud);
            }
            
            

            //Obtengo los datos de los coches del usuario para mostrarlos
            _coche = CD_Auto.MostrarCOCHE(ID());

            if(_coche.Count < 1)
            {
                TempData["NoCars"] = "¡Crea coches nuevos!";
                ViewBag.NoCars = TempData["NoCars"].ToString();
            }

            return View((_coche));
        }

        [HttpPost]
        public JsonResult GetAuthToken(string lati, string longi)
        {
            //Obtengo la ubicacion desde js y la guardo en la session para guardarlas en la bbdd posteriormente
            HttpContext.Session.SetString("Latitud_actualizada", lati);
            HttpContext.Session.SetString("Longitud_actualizada", longi);

            //Si todo ha ido bien,saldra un alert notificandolo
            return Json(new { Msg = "Ubicacion actualizada! Asignela a su vehículo" });
        }

        [HttpPost]
        public ActionResult Index(int idAUTO)
        {
            //https://www.google.com/maps/dir/'@latiActual,-@longiActual'/''/@latiCoche,-@longiCoche,11z/data=!3m1!4b1!4m14!4m13!1m5!1m1!1s0x0:0xc8c36b17074ece53!2m2!1d-0.5068824!2d38.5469467!1m5!1m1!1s0xd623662d742628d:0x402af6ed721d5c0!2m2!1d-0.4909626!2d38.3460627!3e0
            string lati = (string)HttpContext.Session.GetString("Latitud_actualizada");
            string longi = (string)HttpContext.Session.GetString("Longitud_actualizada");

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
