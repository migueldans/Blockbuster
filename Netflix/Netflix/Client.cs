using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Netflix
{
    class Client
    {
        static String connectionString = ConfigurationManager.ConnectionStrings["conexionVideo"].ConnectionString;
        static SqlConnection conexion = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;

        private string firstName, lastName, userName, email, password;
        private DateTime birthDate;
        private bool coincideUser = false;
        private bool coincideMail = false;
        private int newClientId = 0,id;


        public Client(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        public Client (string userName,string firstName,string lastName,DateTime birthDate,string password,string email)
        {
            conexion.Open();

            cadena = "SELECT Max(ClientId) AS 'ClientId' FROM Client";
            comando = new SqlCommand(cadena, conexion);
            SqlDataReader clientId = comando.ExecuteReader();
            if (clientId.Read())
            {
                newClientId = Convert.ToInt32(clientId["ClientId"].ToString()) + 1;
            }
            clientId.Close();
         
            this.userName = userName;
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
            this.password = password;
            this.email = email;

            conexion.Close();
        }
 
        public string GetUserName()
        {
            return userName;
        }
        public void SetUserName(string introUserName)
        {
            this.userName = introUserName;
        }

        public string GetFirstName()
        {
            return firstName;
        }
        public void SetFirstName(string introFirstName)
        {
            this.firstName = introFirstName;
        }

        public string GetLastName()
        {
            return lastName;
        }
        public void SetLastName(string introLastName)
        {
            this.lastName = introLastName;
        }

        public string GetPassword()
        {
            return password;
        }
        public void SetPassword(string introPassword)
        {
            this.password = introPassword;
        }

        public string GetEmail()
        {
            return email;
        }
        public void SetEmail(string introEmail)
        {
            this.email = introEmail;
        }

        public DateTime GetBirthDate()
        {
            return birthDate;
        }
        public void SetBirthDate(DateTime introBirthDate)
        {
            this.birthDate = introBirthDate;
        }

        public void Registro()
        {
            conexion.Open();

            cadena = "INSERT INTO Client VALUES (" + newClientId + ",'" + userName + "','" + firstName + "','" + lastName + "','" + birthDate.ToString("yyyy-dd-MM") + "','" + password + "','" + email + "')";
            comando = new SqlCommand(cadena, conexion);
            comando.ExecuteReader();

            conexion.Close();
        }
        public Boolean Inicio()
        {
            conexion.Open();
            
            cadena = "SELECT * FROM client WHERE UserName like '" + userName + "' and Pass LIKE '" + password + "'";
            comando = new SqlCommand(cadena, conexion);
            SqlDataReader match = comando.ExecuteReader();
            if (match.Read())
            {
                match.Close();
                conexion.Close();
                return true;
            }
            else
            {
                match.Close();
                conexion.Close();
                return false;
            }

        }
        public int SacarEdad()
        {
            conexion.Open();

            cadena= "select convert(int, DATEDIFF(day, (SELECT BirthDate FROM Client WHERE UserName like 'migueldans'), getdate())/ 365.25)";
            comando = new SqlCommand(cadena, conexion);
            SqlDataReader age = comando.ExecuteReader();
            int edad = 0;
            if(age.Read())
            { 
            edad = Convert.ToInt32(age[0].ToString());
            }
            age.Close();
            conexion.Close();
            return edad;
        }
    }

}
