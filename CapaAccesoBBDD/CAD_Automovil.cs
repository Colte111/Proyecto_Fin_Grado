using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CapaAccesoBBDD
{
    public class CAD_Automovil
    {
        private CAD_Conexion conexion = new CAD_Conexion();
        SqlDataReader leer;
        SqlCommand comando = new SqlCommand();

        public DataTable MostrarCOCHE(int id)
        {
            SqlDataReader leer;
            DataTable tabla = new DataTable();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "VER_AUTOMOVIL";
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@id", id);
            leer = comando.ExecuteReader();
            tabla.Load(leer);
            conexion.CerrarConexion();
            return tabla;
        }
        public void InsertarCOCHE(string matricula, string marca, int usuarioid)
        {
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "INSERTAR_AUTOMOVIL";
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@matricula", matricula);
            comando.Parameters.AddWithValue("@marca", marca);
            comando.Parameters.AddWithValue("@usuarioid", usuarioid);

            comando.ExecuteNonQuery();
            comando.Parameters.Clear();
            conexion.CerrarConexion();
        }
        public void EliminarCOCHE(int id)
        {
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "ELIMINARCOCHE";
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@idauto", id);
            comando.ExecuteNonQuery();
            comando.Parameters.Clear();
            conexion.CerrarConexion();
        }
        public void UpdateUbicacionCOCHE(string lat, string long1, int id, int idAUTO)
        {
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "update AUTOMOVIL set Latitud='" + lat + "' , Longitud='" + long1 + "' where AUTOMOVIL.USUARIOid=" + id + " AND AUTOMOVIL.AUTOMOVILid=" + idAUTO + "";
            comando.CommandTimeout = 4;
            comando.CommandType = CommandType.Text;
            comando.ExecuteNonQuery();
            comando.Parameters.Clear();
            conexion.CerrarConexion();
        }
    }
}
