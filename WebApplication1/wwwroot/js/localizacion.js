function iniciar() {
    var boton = document.getElementById('obtener');
    boton.addEventListener('click', obtener, false);
}

function obtener() {
    navigator.geolocation.getCurrentPosition(mostrar, gestionarErrores);
}

function mostrar(posicion) {
    var ubicacion = document.getElementById('localizacion');
    var datos = '';
    datos += 'Latitud: ' + posicion.coords.latitude + '<br>';
    datos += 'Longitud: ' + posicion.coords.longitude + '<br>';
    //ubicacion.innerHTML=datos;
    guardarposicion(posicion);
}

function guardarposicion(posicion) {

    var posicion12 = {
        lat: 1,
        long1: 0
    };
    var latitud = posicion.coords.latitude; //retrieve username from textbox with id 'username'
    var logitud = posicion.coords.longitude;

    $.ajax({
        type: "POST",
        url: '/Auto/GetAuthToken',
        dataType: "json",
        data: { lati: latitud, longi: logitud },
        success: function (result, status, xhr) {
            alert(result.msg);
        },
        error: function (xhr, status, error) {
            alert("Error");
        }
    });
}

function gestionarErrores(error) {
    alert('Error: ' + error.code + ' ' + error.message + '\n\nPor favor compruebe que está conectado ' +
        'a internet y habilite la opción permitir compartir ubicación física');
}
window.addEventListener('load', iniciar, false);
