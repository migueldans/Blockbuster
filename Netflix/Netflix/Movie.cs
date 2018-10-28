using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Netflix
{
    class Movie
    {
        static String connectionString = ConfigurationManager.ConnectionStrings["conexionVideo"].ConnectionString;
        static SqlConnection conexion = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;

        private int num;

        public Movie(int num)
        {
        }
        
        public void MostrarPelicula()
        {
            conexion.Open();
            int num;

            cadena = "SELECT  FROM Movie WHERE MovieId ='" + num + "'";
            comando = new SqlCommand(cadena, conexion);
            SqlDataReader films = comando.ExecuteReader();

            conexion.Close();
        }

        
    }

}
