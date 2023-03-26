using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAccesoBBDD
{
    public class CAD_Usuario
    {
        private CAD_Conexion conexion = new CAD_Conexion();
        SqlDataReader leer;
        DataTable tabla = new DataTable();
        SqlCommand comando = new SqlCommand();
        public DataTable Mostrar()
        {
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "selectusuario";
            comando.CommandType = CommandType.StoredProcedure;
            leer = comando.ExecuteReader();
            tabla.Load(leer);
            conexion.CerrarConexion();
            return tabla;
        }

        public void UsuarioNuevo(string nombre, string apellidos, string correo, string fechanac, string genero, string contraseña)
        {
            comando.Connection = conexion.AbrirConexion();
            comando.CommandText = "insertarusu";
            comando.CommandType = CommandType.StoredProcedure;
            comando.Parameters.AddWithValue("@nombre", nombre);
            comando.Parameters.AddWithValue("@ape", apellidos);
            comando.Parameters.AddWithValue("@correo", correo);
            comando.Parameters.AddWithValue("@fechanac", fechanac);
            comando.Parameters.AddWithValue("@genero", genero);
            comando.Parameters.AddWithValue("@contra", contraseña);

            comando.ExecuteNonQuery();
            comando.Parameters.Clear();
            conexion.CerrarConexion();
        }
        
    }
}
