using System;
using CapaAccesoBBDD;
using System.Data;
using Modelos;
using System.Collections.Generic;

namespace CapaDominio
{
    public class CD_Usuario
    {
        private CAD_Usuario objetoCD = new CAD_Usuario();

        public List<USUARIO> ListaUsuarios()
        {
            List<USUARIO> list = new List<USUARIO>();
            DataTable usuario = new DataTable();
            usuario = objetoCD.Mostrar();
            foreach(DataRow dr in usuario.Rows)
            {
                list.Add(new USUARIO(dr));
            }
            return list;
        }
        public USUARIO ValidarUsuario(string _correo, string _clave)
        {
            USUARIO user = new USUARIO();
            user = ListaUsuarios().Where(item => item.Correo == _correo && item.Constraseña == _clave).FirstOrDefault();
            return user;
        }

        public void InsertarUsuario(string nombre, string apellidos, string correo, string fechanac, string genero,string contraseña)
        {

            objetoCD.UsuarioNuevo(nombre, apellidos, correo, fechanac, genero, contraseña);
        }

       
       
    }
}
