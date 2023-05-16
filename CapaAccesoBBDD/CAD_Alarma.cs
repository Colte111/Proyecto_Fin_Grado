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
        private SqlDataReader leer;
        private SqlCommand comando = new SqlCommand();
        public void InsertarAlarma(int AUTOMOVILid, DateTime fecha)
        {
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "INSERTAR_ALARMA";
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@fecha", fecha);
            comando.Parameters.AddWithValue("@AUTOMOVILid", AUTOMOVILid);

            comando.ExecuteNonQuery();
            comando.Parameters.Clear();
            conexion.CerrarConexion();
        }

        public DataTable ObtenerAlarma(int id)
        {
            DataTable tabla = new DataTable();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText =
                " SELECT a.ALARMAid,a.AUTOMOVILid,a.FECHA " +
                " FROM ALARMA a, AUTOMOVIL au " +
                " WHERE au.AUTOMOVILid = " + id +
                " and au.AUTOMOVILid = a.AUTOMOVILid " +
                " and a.FECHA > GETDATE() ";

            comando.CommandTimeout = 2;
            comando.CommandType = CommandType.Text;

            leer = comando.ExecuteReader();
            tabla.Load(leer);
            conexion.CerrarConexion();

            return tabla;
        }
        public DataTable PrimeraAlarma(int id)
        {
            DataTable tabla = new DataTable();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "SELECT TOP(1) a.Fecha,a.AUTOMOVILid,a.ALARMAid " +
                                    " FROM ALARMA a,AUTOMOVIL au " +
                                    " WHERE a.Fecha >= GETDATE() " +
                                    " and au.USUARIOid = " + id +
                                    " and au.AUTOMOVILid = a.AUTOMOVILid " +
                                    " ORDER BY a.Fecha ASC";
            
            comando.CommandTimeout = 2;
            comando.CommandType = CommandType.Text;

            leer = comando.ExecuteReader();
            tabla.Load(leer);
            conexion.CerrarConexion();

            return tabla;
        }
        public void EliminarAlarma(int ALARMAid)
        {
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "ELIMINARALARMA";
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@ALARMAid", ALARMAid);
            comando.ExecuteNonQuery();
            comando.Parameters.Clear();
            conexion.CerrarConexion();
        }
        public void ActualizarAlarma(int ALARMAid,DateTime fecha)
        {
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "ACTUALIZARALARMA";
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@ALARMAid", ALARMAid);
            comando.Parameters.AddWithValue("@fecha", fecha);
            comando.ExecuteNonQuery();
            comando.Parameters.Clear();
            conexion.CerrarConexion();
        }
    }
}
