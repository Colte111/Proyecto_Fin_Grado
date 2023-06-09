﻿using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Modelos
{
    public class USUARIO
    {
        public int USUARIOid { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Constraseña { get; set; }

        [Display(Name = "DD/MM/YYYY")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$",
            ErrorMessage = "Formato incorrecto")]
        public string FechaNacimiento { get; set; }
        public string Genero { get; set; }

        public USUARIO()
        {

        }
        public USUARIO(DataRow row)
        {
            USUARIOid = (int)row["USUARIOid"];
            Nombre = (string)row["Nombre"];
            Apellidos = (string)row["Apellidos"];
            Correo = (string)row["Correo"];
            Constraseña = (string)row["contraseña"];
        }
    }
}
