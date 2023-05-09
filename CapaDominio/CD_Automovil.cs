using CapaAccesoBBDD;
using Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capaDominio
{
    public class CD_Automovil
    {
        private CAD_Automovil objetoCD = new CAD_Automovil();
        public void updatecoords(string lat, string long1, int id, int idauto)
        {
            objetoCD.UpdateUbicacionCOCHE(lat, long1, id, idauto);
        }

        public void EliminarCOCHE(int id)
        {
            objetoCD.EliminarCOCHE(id);
        }

        public List<AUTOMOVIL> MostrarCOCHE(int id)
        {
            List<AUTOMOVIL> coche = new List<AUTOMOVIL>();
            DataTable _coche = new DataTable();
            _coche = objetoCD.MostrarCOCHE(id);
            foreach (DataRow row in _coche.Rows)
            {
                coche.Add(new AUTOMOVIL(row));
            }
            return coche;
        }

        public void InsertarCOCHE(string matricula, string marca, int usuarioid)
        {
            objetoCD.InsertarCOCHE(matricula, marca, usuarioid);
        }
    }
}
