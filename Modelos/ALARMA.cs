using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Modelos
{
    public class ALARMA
    {
        public int ALARMAid { get; set; }
        public DateTime Fecha { get; set; }

        public ALARMA()
        {

        }
        public ALARMA(DataRow row)
        {
            Fecha = (DateTime)row["Fecha"];
        }
    }
}
