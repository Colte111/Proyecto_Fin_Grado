using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Modelos
{
    public class ALARMA
    {
        public int ALARMAid { get; set; }
        public DateTime fecha { get; set; }

        public ALARMA()
        {

        }
        public ALARMA(DataRow row)
        {
            fecha = (DateTime)row["FECHA"];
        }

        public DateTime GetAlarma()
        {
            return fecha;
        }

    }
}
