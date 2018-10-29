using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Netflix
{
    class Rent
    {
        static String connectionString = ConfigurationManager.ConnectionStrings["conexionVideo"].ConnectionString;
        static SqlConnection conexion = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;

        private int numAlquiler;
        private int newRentId = 0;
        private string userName;

        public Rent(int numAlquiler, string userName)
        {
            conexion.Open();
            cadena = "SELECT Max(RentId) AS 'RentId' FROM Rent";
            comando = new SqlCommand(cadena, conexion);
            SqlDataReader rentId = comando.ExecuteReader();
            if (rentId.Read())
            {
                newRentId = Convert.ToInt32(rentId["RentId"].ToString()) + 1;
            }
            rentId.Close();

            this.numAlquiler = numAlquiler;
            this.userName = userName;
        }

        public void AlquilarPelicula()
        {
            conexion.Close();
            conexion.Open();

            cadena = "INSERT INTO Rent VALUES ('" + newRentId + "', (select clientid from Client where username ='" + userName + "'),'" + numAlquiler + "',getdate(),DATEADD(dd,3, GETDATE()))";
            comando = new SqlCommand(cadena, conexion);
            comando.ExecuteReader();
            conexion.Close();

            conexion.Open();
            cadena = "UPDATE movie SET statemovie=1 where movieid='" + numAlquiler + "'";
            comando = new SqlCommand(cadena, conexion);
            comando.ExecuteReader();
            conexion.Close();

            conexion.Open();
            cadena = "SELECT title FROM Movie WHERE movieId='" + numAlquiler + "'";
            comando = new SqlCommand(cadena, conexion);
            SqlDataReader titulo = comando.ExecuteReader();
            while (titulo.Read())
            {
                Console.WriteLine("Has alquilado la pelicula " + titulo["title"].ToString());
            }
            Console.ReadLine();
            conexion.Close();
        }

        public void PeliculasAlquiladas()
        {
            conexion.Close();
            conexion.Open();

            cadena = "SELECT r.clientid, r.RentFrom, r.RentTo, r.movieid, m.title FROM Rent r, Movie m WHERE r.clientId = (SELECT clientId FROM Client WHERE UserName like '" + userName + "') and r.movieid = m.movieid and m.StateMovie = 1";
            comando = new SqlCommand(cadena, conexion);
            SqlDataReader alquiladas = comando.ExecuteReader();
            while (alquiladas.Read())
            {
                if (DateTime.Parse(alquiladas["RentTo"].ToString()) <= DateTime.Now)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(alquiladas["MovieId"].ToString() + " " + alquiladas["Title"] + " " + alquiladas["RentFrom"] + " " + alquiladas["RentTo"]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                Console.WriteLine(alquiladas["MovieId"].ToString() + " " + alquiladas["Title"] + " " + alquiladas["RentFrom"] + " " + alquiladas["RentTo"]);
            }
        }
            Console.ReadLine();
            alquiladas.Close();
            conexion.Close();
        }

        public void DevolverPeliculas(int numDevolver)
        {
            conexion.Close();
            conexion.Open();
            cadena = "UPDATE movie SET statemovie=0 where movieid='" + numDevolver + "'";
            comando = new SqlCommand(cadena, conexion);
            comando.ExecuteReader();
            conexion.Close();
        }
    }
}
