using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Netflix
{
    class Program
    {

        static String connectionString = ConfigurationManager.ConnectionStrings["ConexionVideo"].ConnectionString;
        static SqlConnection conexion = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;

        static void Main(string[] args)
        {

            Menu();

        }

        public static void Menu()
        {
            int numSelected;

            Console.WriteLine("\nBienvenido a Blockbuster, el futuro en el arrendamiento de películas.");

            do
            {
                Console.WriteLine("\n1.Registrarse");
                Console.WriteLine("\n2.Iniciar Sesión");
                Console.WriteLine("\n3.Salir");
                numSelected = Convert.ToInt32(Console.ReadLine());

                try
                {
                    numSelected = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Introduzca un numero del menú.");
                }
            }
            while (numSelected <= 0 || numSelected > 3);
            
            switch (numSelected)
            {
                case 1:
                    Registrarse();
                    break;
                case 2:
                    IniciarSesion();
                    break;
                case 3:
                    Salir();
                    break;
            }
            
        }
    }
}
