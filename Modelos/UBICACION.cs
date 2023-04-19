using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Modelos
{
    public class UBICACION
    {
        public string Latitud { get; set; }
        public string Longitud { get; set; }

        public UBICACION() { }
        
        public UBICACION(DataRow dr)
        {
            Latitud = (string)dr["Latitud"];
            Longitud = (string)dr["Longitud"];
        }
    }
}
