using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Modelos
{
    public class ALARMA
    {
        public int ALARMAid { get; set; }
        public int AUTOMOVILid { get; set; }
        public DateTime Fecha { get; set; }

        public ALARMA()
        {

        }
        public ALARMA(DataRow row)
        {
            AUTOMOVILid = (int)row["AUTOMOVILid"];
            ALARMAid = (int)row["ALARMAid"];
            Fecha = (DateTime)row["Fecha"];
        }
    }
}
