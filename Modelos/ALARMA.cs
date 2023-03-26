using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Modelos
{
    public class ALARMA
    {
        public int ALARMAid { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public string Nombre { get; set; }
        [Display(Name = "Fecha de Alarma")]

        public DateTime DOB { get; set; }

        public ALARMA()
        {

        }
        public ALARMA(DataRow row)
        {

            DOB = (DateTime)row["FECHA"];
            
            this.fecha = DOB.ToString();
        }

    }
}
