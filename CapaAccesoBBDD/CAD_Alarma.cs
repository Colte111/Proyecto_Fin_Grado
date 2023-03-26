using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAccesoBBDD
{
    public  class CAD_Alarma
    {
        private CAD_Conexion conexion = new CAD_Conexion();
        SqlDataReader leer;
        SqlCommand comando = new SqlCommand();
        public void InsertarAlarma(int id, DateTime fecha)//,DateTime hora)
        {
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "INSERTAR_ALARMA";
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@fecha", fecha);
            comando.Parameters.AddWithValue("@USUARIOid", id);
            //comando.Parameters.AddWithValue("@hora", hora);

            comando.ExecuteNonQuery();
            comando.Parameters.Clear();
            conexion.CerrarConexion();
        }

        public DataTable ObtenerAlarma(int id)
        {
            DataTable tabla = new DataTable();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "Select a.FECHA,concat(u.Nombre,' ',u.Apellidos) as Nombre from ALARMA a,USUARIO u WHERE " +
                "u.USUARIOid=" + id + " and a.USUARIOid=u.USUARIOid";
            comando.CommandTimeout = 2;
            comando.CommandType = CommandType.Text;
            leer = comando.ExecuteReader();
            tabla.Load(leer);
            conexion.CerrarConexion();
            return tabla;
        }
    }
}
