using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Modelos
{
    public class USUARIO
    {
        public int USUARIOid { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre")]

        public string Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Apellidos")]

        public string Apellidos { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Este campo es requerido.")]
        [DataType(DataType.Password)]
        public string Constraseña { get; set; }
        

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]

        public string FechaNacimiento { get; set; }

        [Display(Name = "Genero")]

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
