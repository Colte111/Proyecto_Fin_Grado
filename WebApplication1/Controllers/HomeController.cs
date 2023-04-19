using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;


using Microsoft.AspNetCore.Authorization;
using CapaDominio;
using MessagePack;
using Modelos;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    //2.- AÑADIR LA AUTHORIZACION --> si no hay usuario logeado, no se podra acceder a la web
    [Authorize]
    public class HomeController : Controller
    {
        CD_Alarma cd_alarma = new CD_Alarma();
        private readonly ILogger<HomeController> _logger;
        public IHubContext<AlarmHub> _alarmHubContext;

        public HomeController(IHubContext<AlarmHub> alarmHubContext)
        {
            _alarmHubContext = alarmHubContext;
        }
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

        public async Task StartAlarmAsync(DateTime alarmTime)
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

        public IActionResult Index()
        {
            StartAlarmAsync(ALARMA());
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}