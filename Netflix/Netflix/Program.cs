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
        static String connectionString = ConfigurationManager.ConnectionStrings["conexionVideo"].ConnectionString;
        static SqlConnection conexion = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;
        static Client cliente;

        static void Main(string[] args)
        {

            Menu();

        }

        public static void Menu()
        {
            int numSelected = 0;

            Console.WriteLine("\nBienvenido a Blockbuster, el futuro en el arrendamiento de películas.");

            do
            {
                try
                {
                    Console.WriteLine("\n1.Registrarse");
                    Console.WriteLine("\n2.Iniciar Sesión");
                    Console.WriteLine("\n3.Salir");
                    numSelected = Convert.ToInt32(Console.ReadLine());

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
                catch (FormatException ex)
                {
                    Console.WriteLine("Error, introduzca un numero del menú.");

                }
            } while (numSelected <= 0 || numSelected > 3);

        }

        public static void Registrarse()
        {
            conexion.Open();

            string firstName, lastName, userName, email, password;
            DateTime birthDate;
            SqlDataReader match;
            bool coincideUser = false;
            bool coincideMail = false;
            //int newClient = 0;

            Console.WriteLine("\n Introduzca su nombre:");
            firstName = Console.ReadLine();

            Console.WriteLine("\n Introduzca su apellido:");
            lastName = Console.ReadLine();

            do
            {
                if (coincideUser == true)
                {
                    Console.WriteLine("\n El nombre de usuario no está disponible, por favor introduzca otro:");
                    userName = Console.ReadLine();
                }
                Console.WriteLine("\n Introduzca su nombre de usuario:");
                userName = Console.ReadLine();
                cadena = "SELECT * FROM Client WHERE UserName like '" + userName + "'";
                comando = new SqlCommand(cadena, conexion);
                match = comando.ExecuteReader();
                if (match.Read())
                {
                    coincideUser = true;
                }
                else
                {
                    coincideUser = false;
                }
                match.Close();
            }
            while (coincideUser == true);

            do
            {
                if (coincideMail == true)
                {
                    Console.WriteLine("\n Ya se ha registrado con ese mail, por favor introduzca otro:");
                }
                Console.WriteLine("\n Introduzca su correo electrónico:");
                email = Console.ReadLine();
                cadena = "SELECT * FROM Client WHERE email like'" + email + "'";
                comando = new SqlCommand(cadena, conexion);
                match = comando.ExecuteReader();
                if (match.Read())
                {
                    coincideMail = true;
                }
                else
                {
                    coincideMail = false;
                }
                match.Close();
            }
            while (coincideMail == true);

            Console.WriteLine("\n Introduzca su contraseña:");
            password = Console.ReadLine();

            Console.WriteLine("\n Introduzca su fecha de nacimiento en formato dia/mes/año:");
            birthDate = Convert.ToDateTime(Console.ReadLine());

            cliente = new Client(userName, firstName, lastName, birthDate, password, email);
            cliente.Registro();
            Menu();
            conexion.Close();
        }

        public static void IniciarSesion()
        {
            conexion.Close();
            conexion.Open();

            Console.WriteLine("\n Inicio de sesión");
            Console.WriteLine("----------------");
            bool inicio = false;
            //SqlDataReader match;
            string userName;
            do
            {
                Console.WriteLine("\n Introduzca su nombre de usuario");
                userName = Console.ReadLine();
                Console.WriteLine("\n Introduzca su contraseña.");
                string password = Console.ReadLine();

                cliente = new Client(userName, password);
                if (cliente.Inicio() == true)
                {
                    inicio = true;
                }
                else
                {
                    inicio = false;
                    Console.WriteLine("Su nombre de usuario o contraseña es incorrecto.");
                }

            }
            while (inicio == false);

            conexion.Close();
            MenuRent();
            //TODO aqui hay o en la clase client hay que sacar el id del cliente y guardarlo en una variable,que usaremos despues en el aquiler de peliculas
        }

        public static void Salir()
        {

        }

        public static void MenuRent()
        {
            int numSelected = 0;

            Console.WriteLine("\n¿Qué desea hacer?.");

            do
            {
                try
                {
                    Console.WriteLine("\n1.Peliculas disponibles");
                    Console.WriteLine("\n2.Alquilar película(s)");
                    Console.WriteLine("\n3.Mis peliculas alquiladas");
                    Console.WriteLine("\n4.Cerrar sesión");
                    numSelected = Convert.ToInt32(Console.ReadLine());

                    switch (numSelected)
                    {
                        case 1:
                            PeliculasDisponibles();
                            break;
                        case 2:
                            AlquilarPeliculas();
                            break;
                        case 3:
                            MisPeliculasAlquiladas();
                            break;
                        case 4:
                            CerrarSesion();
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Error, introduzca un numero del menú.");

                }
            } while (numSelected <= 0 || numSelected > 4);
        }

        public static void PeliculasDisponibles()
        {
            conexion.Open();
            int edad = cliente.SacarEdad();
            cadena= "SELECT * FROM Movie WHERE ContentRating <='"+edad+"'";
            comando = new SqlCommand(cadena, conexion);
            SqlDataReader films = comando.ExecuteReader();
            while (films.Read())
            {
                Console.WriteLine(films["MovieId"].ToString() +" "+ films["Title"]);
            }
            try 
            {
                Console.WriteLine("Escriba un número para ver la sinopsis de la película elegida");
                int num = Convert.ToInt32(Console.ReadLine());

            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error, introduzca un número válido.");
            }
            //catch (Exception ex2)
            //{
            //    Console.WriteLine("Error, introduzca un número válido.");
            //}
            Console.ReadLine();
            films.Close();
            conexion.Close();

        }
        public static void AlquilarPeliculas()
        {
            conexion.Open();
            int edad = cliente.SacarEdad();
            cadena = "SELECT * FROM Movie WHERE ContentRating <='" + edad + "' and StateMovie = 0";
            comando = new SqlCommand(cadena, conexion);
            SqlDataReader films = comando.ExecuteReader();
            while (films.Read())
            {
                Console.WriteLine(films["MovieId"].ToString() + " " + films["Title"]);
            }
            films.Close();
            conexion.Close();
        }
        public static void MisPeliculasAlquiladas()
        {
            conexion.Open();
            cadena = "SELECT * FROM Movie WHERE ContentRating <='" + edad + "' and StateMovie = 0";
            comando = new SqlCommand(cadena, conexion);
            SqlDataReader films = comando.ExecuteReader();
            while (films.Read())
            {
                Console.WriteLine(films["MovieId"].ToString() + " " + films["Title"]);
            }
            films.Close();
            conexion.Close();
        }
        public static void CerrarSesion()
        {

        }
    }
}
