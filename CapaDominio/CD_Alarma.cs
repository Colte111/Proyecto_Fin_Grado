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

        public List<ALARMA> _obtenerAlarma(int id)
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
    }
}
