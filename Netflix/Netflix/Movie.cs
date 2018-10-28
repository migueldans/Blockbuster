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
            this.num = num;
        }
        
        public void MostrarPelicula()
        {
            conexion.Open();

            cadena = "SELECT title, director, releasedate, contentrating, synopsis FROM Movie WHERE MovieId ='" + num + "'";
            comando = new SqlCommand(cadena, conexion);
            SqlDataReader films = comando.ExecuteReader();
            while (films.Read())
            {
                Console.WriteLine(films["Title"]+"\nDirector: "+ films["director"] + "\nFecha de estreno: " + films["releasedate"]  +"\nEdadRecomendada: " + films["contentrating"]+"\nSinopsis: " + films["synopsis"]);
            }
            Console.WriteLine("\nPulse enter para volver al menú principal.");
            Console.ReadLine();
            conexion.Close();
        }


        
    }

}
