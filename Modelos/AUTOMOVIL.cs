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
        public string lat { get; set; }
        public string long1 { get; set; }


        public AUTOMOVIL()
        {

        }
        public AUTOMOVIL(DataRow dr)
        {
            AUTOMOVILid = (int)dr["AUTOMOVILid"];
            Marca = (string)dr["Marca"];
            Matricula = (string)dr["Matricula"];
            lat = (string)dr["lat1"];
            long1 = (string)dr["long"];
            //Modelo = (string)dr["Modelo"];
        }
    }
}
