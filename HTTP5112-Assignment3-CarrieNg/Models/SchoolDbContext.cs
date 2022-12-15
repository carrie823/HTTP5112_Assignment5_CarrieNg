using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace HTTP5112_Assignment3_CarrieNg.Models
{
    public class SchoolDbContext
    {
        
        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "school"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }

        
        protected static string ConnectionString
        {
            get
            {
               
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
            }
        }
        /// <summary>
        /// Returns connection to the schooldb database.
        /// </summary>
        /// <example>
        /// private SchoolDbContext school = new SchoolDbContext();
        /// MySqlConnection Conn = school.AccessDatabase();
        /// </example>
        /// <returns>
        /// A MySqlConnection Object
        /// </returns>
        public MySqlConnection AccessDatabase()
        {
            //creating a MySqlConnection to create an object
            //the object is connection to the school database on port 3306 of localhost
            return new MySqlConnection(ConnectionString);
        }
    }
}