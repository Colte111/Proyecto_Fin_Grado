const connection = new signalR.HubConnectionBuilder()
    .withUrl("/alarmHub")
    .build();

function ocultar() {
    document.getElementById("iniciarRuta").style.visibility = "hidden";
}
connection.start().then(function () {
    console.log("Alarma sonando");
});

connection.on("alarm", () => {
    // Mostramos la ventana emergente
    alert("¡Hora de ir a por tu coche!");
    document.getElementById("iniciarRuta").style.visibility = "visible";
});

document.getElementById("iniciarRuta").addEventListener('click',ocultar());
connection.start();