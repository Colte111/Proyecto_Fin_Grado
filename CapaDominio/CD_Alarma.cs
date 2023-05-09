using CapaAccesoBBDD;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDominio
{
    public class CD_Alarma
    {
        private CAD_Alarma cad_alarma = new CAD_Alarma();

        public List<ALARMA> _obtenerListAlarma(int id)
        {
            List<ALARMA> _alarma = new List<ALARMA>();
            DataTable dt = new DataTable();

            dt = cad_alarma.ObtenerAlarma(id);

            foreach(DataRow dr in dt.Rows)
            {
                _alarma.Add(new ALARMA(dr));
            }

            return _alarma;
        }

        public ALARMA _primeraAlarma(int id)
        {
            ALARMA _alarma = new ALARMA();
            DataTable dt = new DataTable();

            dt = cad_alarma.PrimeraAlarma(id);

            foreach (DataRow dr in dt.Rows)
            {
                _alarma = new ALARMA(dr);
            }

            return _alarma;
        }

        public void _insertarAlarma(int AUTOMOVILid,DateTime fecha)
        {
            cad_alarma.InsertarAlarma(AUTOMOVILid, fecha);
        }
        public void _eliminarAlarma(int ALARMAid)
        {
            cad_alarma.EliminarAlarma(ALARMAid);
        }
        public void _actualizarAlarma(int ALARMAid,DateTime fecha)
        {
            cad_alarma.ActualizarAlarma(ALARMAid, fecha);
        }
    }
}
