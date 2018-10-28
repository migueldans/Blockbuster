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
            
            DateTime thisDay = DateTime.Today;
            thisDay = Convert.ToDateTime(Console.ReadLine());
            DateTime newDate = thisDay.AddDays(3);
            newDate = Convert.ToDateTime(Console.ReadLine());
            cadena = "INSERT INTO Rent VALUES ('" + newRentId + "','" + "select clientid from Client where username ='"+userName+"' " + "','" + numAlquiler + "','" + thisDay + "','" + newDate + "')";
            comando = new SqlCommand(cadena, conexion);
            comando.ExecuteReader();

            conexion.Close();
        }
    }
}
