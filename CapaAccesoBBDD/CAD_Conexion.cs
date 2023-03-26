using System.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.IO;

namespace CapaAccesoBBDD
{
    public class CAD_Conexion
    {
        private SqlConnection Conexion;
        private bool Isconected = false;
        public SqlConnection AbrirConexion()
        {
            if (!Isconected)
            {
                createConection();
                Isconected = true;
            }

            if (Conexion.State == ConnectionState.Closed)
                Conexion.Open();
            return Conexion;
        }
        public SqlConnection CerrarConexion()
        {
            if (Conexion.State == ConnectionState.Open)
                Conexion.Close();
            return Conexion;
        }

        private void createConection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var db = builder.Build().GetSection("ConnectionStrings").GetSection("CARMAPS").Value;

            Conexion = new SqlConnection(db);
        }
    }
}