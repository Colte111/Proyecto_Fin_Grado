//1.- PRIMERO =================================================================
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplication1;
using WebApplication1.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//Servicio del Hub
builder.Services.AddSignalR();


//2.- SEGUNDO - CONFIGURAR NUESTRA COOKIE DE AUTENTICACION =================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option => {
        option.LoginPath = "/Acceso/Index";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        option.AccessDeniedPath = "/Home/Privacy";
    });

//3.- CONFIGURACIÓN PARA USAR SESIONES
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();



app.UseRouting();

//3.- TERCERO ==================================================================
app.UseAuthentication();

app.UseAuthorization();
app.UseSession();

//Servicio del hub
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<AlarmHub>("/alarmHub");
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Index}/{id?}");

app.Run();
