using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection.Emit;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.SignalR;
using WebApplication1;
using Microsoft.AspNetCore.Mvc;
using CapaDominio;
using Modelos;
using System.Security.Claims;

public class AlarmService 
{
    private readonly IHubContext<AlarmHub> _hubContext;

    public AlarmService(IHubContext<AlarmHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task StartAlarmAsync(DateTime alarmTime)
    {
        // Obtener la hora actual y calcular la cantidad de segundos hasta que suene la alarma
        var currentTime = DateTime.Now;
        var timeToAlarm = (int)(alarmTime - currentTime).TotalSeconds;

        // Esperar hasta que sea hora de sonar la alarma
        await Task.Delay(timeToAlarm * 1000);

        // Enviar el mensaje a través del concentrador de SignalR
        await _hubContext.Clients.All.SendAsync("alarm");
    }
}

