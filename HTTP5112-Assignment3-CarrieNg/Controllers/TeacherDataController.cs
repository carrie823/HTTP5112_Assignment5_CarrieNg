using HTTP5112_Assignment3_CarrieNg.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace HTTP5112_Assignment3_CarrieNg.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext school = new SchoolDbContext();

        /// <summary>
        /// contacts the database and returns articles in the teachers table. Search Bar can be used to seach for specific teachers by their first and lastnames
        /// </summary>
        /// <example> 
        /// GET: api/TeacherData/ListTeachers/{SearchKey?} -> "Linda Chan"
        /// </example>
        /// <returns>
        /// a list of all the teachers first and last names, when you select each teacher you can see all information of teacher from database
        /// </returns>

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {
            //Create a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the webserver and database -> School database
            Conn.Open();

            //make a command/query for the database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat (teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultsSet = cmd.ExecuteReader();

            //Create an empty list of teacher Names
            List<Teacher> Teachers = new List<Teacher>();

            //Loop through each row the Result Set
            while (ResultsSet.Read())
            {
                int TeacherId = (int)ResultsSet["teacherid"];
                string TeacherFname = (string)ResultsSet["teacherfname"];
                string TeacherLname = (string)ResultsSet["teacherlname"];
                string EmpolyeeNumber = (string)ResultsSet["employeenumber"];
                double Salary = Double.Parse((ResultsSet["salary"].ToString()));
                
                DateTime HireDate = (DateTime)ResultsSet["hiredate"];

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmpolyeeNumber;
                NewTeacher.Salary = Salary;
                NewTeacher.HireDate = HireDate;
                //string TeacherName = ResultsSet["teacherfname"] + " " + ResultsSet["teacherlname"];

                Teachers.Add(NewTeacher);

            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers
            return Teachers;

        }
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create a connection
            MySqlConnection Conn = school.AccessDatabase();

            //Open the connection between the webserver and database
            Conn.Open();

            //make a command/query for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where teacherid ="+id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultsSet = cmd.ExecuteReader();

            //Loop through each row the Result Set
            while (ResultsSet.Read())
            {
                int TeacherId = (int)ResultsSet["teacherid"];
                string TeacherFname = (string)ResultsSet["teacherfname"];
                string TeacherLname = (string)ResultsSet["teacherlname"];
                string EmpolyeeNumber = (string)ResultsSet["employeenumber"];
                double Salary = Double.Parse((ResultsSet["salary"].ToString()));
                DateTime HireDate = (DateTime)ResultsSet["hiredate"];

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmpolyeeNumber;
                NewTeacher.Salary = Salary;
                NewTeacher.HireDate = HireDate;
            }

            //Return the final list of teachers
            return NewTeacher;

        }
        /// <summary>
        /// Delete a Teacher from our database
        /// </summary>
        /// <param name="id"></param>
        /// <example>POST : /api/TeacherData/DeleteTeacher/3/</example>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //create an instance of connection
            MySqlConnection conn = school.AccessDatabase();

            //open the connection between the web server and the database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();

            //Sql Query
            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            conn.Close();

        }

        /// <summary>
        /// Add a Teacher from our database
        /// </summary>
        /// <example>inputing teacher first and lastname, employee number, hire date, salary into form.
        /// and teacher will be added to database and their name will be visiable in Teacher/List/</example>

        [HttpPost]
        public void AddTeacher([FromBody]Teacher NewTeacher)
        {
            //create an instance of connection
            MySqlConnection conn = school.AccessDatabase();

            //open the connection between the web server and the database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();

            //Sql Query
            cmd.CommandText = "Insert into teachers (teacherfname, teacherlname, hiredate, employeenumber, salary) values (@TeacherFname, @TeacherLname, @HireDate, @EmployeeNumber, @Salary)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Update a Teacher in the database. Non-Deterministic.
        /// </summary>
        /// <param name="TeacherInfo">An object field that map to the columns of the Teacher Table</param>
        /// <example>
        /// POST api/TeacherData/UpdateTeacher/5
        /// FORM DATA/POST DATA/REQUEST BODY
        /// {
        /// TeacherFname: "JESSICA"
        /// TeacherLname: "MORRIS"
        /// EmployeeNumber: "T389"
        /// HireDate: "2012-12-08"
        /// Salary: "48.62"
        /// }
        /// </example>
        /// 
        ///Updating using Command Line:
        ///Using curl request with a JSON object to update the teacher data through WebAPI
        ///<example>
        ///-H "Content-Type:application/json" -d @Teacher.json "http://localhost:58704/api/TeacherData/UpdateAuthor/5"
        ///  </example>
        /// <return>
        /// TeacherFname: "JESSICA"
        /// TeacherLname: "MORRIS"
        /// EmployeeNumber: "T389"
        /// HireDate: "2012-12-08"
        /// Salary: "48.62"
        /// </return>


        public void UpdateTeacher(int id, [FromBody]Teacher TeacherInfo)
        {

            //create an instance of connection
            MySqlConnection conn = school.AccessDatabase();

            //Debug.WriteLine(TeacherInfo.TeacherFname);

            //open the connection between the web server and the database
            conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = conn.CreateCommand();

            //Sql Query
            cmd.CommandText = "Update teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname, hiredate=@HireDate, employeenumber=@EmployeeNumber, salary=@Salary where teacherid=@TeacherId";
            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", TeacherInfo.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", TeacherInfo.HireDate);
            cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@TeacherId", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            conn.Close();
        }

    }
}
