using System;
using System.Data;


namespace Modelos
{
    public class AUTOMOVIL
    {
        public int AUTOMOVILid { get; set; }

        public string Marca { get; set; }
        public string Matricula { get; set; }
        public string Modelo { get; set; }
        public string Consumible { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }


        public AUTOMOVIL()
        {

        }
        public AUTOMOVIL(DataRow dr)
        {
            AUTOMOVILid = (int)dr["AUTOMOVILid"];
            Marca = (string)dr["Marca"];
            Matricula = (string)dr["Matricula"];
            Latitud = (string)dr["Latitud"];
            Longitud = (string)dr["Longitud"];
            //Modelo = (string)dr["Modelo"];
        }
    }
}
