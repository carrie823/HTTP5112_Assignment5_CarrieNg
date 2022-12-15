using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP5112_Assignment3_CarrieNg.Models;


namespace HTTP5112_Assignment3_CarrieNg.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }
        //GET: /Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            return View(Teachers);
        }

        //GET : /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();     
            Teacher SelectedTeacher = controller.FindTeacher(id);
          

            return View(SelectedTeacher);
        }

        //GET : /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);


            return View(NewTeacher);
        }

        //POST : /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //GET : /Teacher/New
        public ActionResult New()
        {
            return View();
        }

        //POST : /Teacher/Create
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, DateTime HireDate, string EmployeeNumber, double Salary)
        {
            //Identify that this method is running
            //Identify the input provided from the form

            Debug.WriteLine("I have accessed the create method");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(HireDate);
            Debug.WriteLine(EmployeeNumber);
            Debug.WriteLine(Salary);

            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.HireDate = HireDate;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);


            return RedirectToAction("List");
        }

        /// <summary>
        /// Generates a teacher update page. Getting the information from the database
        /// </summary>
        /// <param name="id">ID of the Teacher</param>
        /// <returns>A dynamic update teacher webpage which has the current information of the teacher,
        /// and allows users to input new information into the form</returns>
        /// <example>GET: /Teacher/Update/{id}</example>

        //GET : /Teacher/Update/{id}

        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher SelectedTeacher = controller.FindTeacher(id);

            return View(SelectedTeacher);
        }

        /// <summary>
        /// Recieve a POST request containing information about an existing teacher in the system with new values. 
        /// Sends this information to the API and directs the page to Show Teacher
        /// </summary>
        /// <param name="id">ID of Teacher</param>
        /// <param name="TeacherFname">Update first name of Teacher</param>
        /// <param name="TeacherLname">Update last name of teacher</param>
        /// <param name="HireDate">Update hire date of Teacher</param>
        /// <param name="EmployeeNumber">Update employee number of Teacher</param>
        /// <param name="Salary">Update salary of Teacher</param>
        /// <returns> a dynamic webpage with the updated information of the Teacher</returns>
        /// <example>POST: /Teacher/Update/5
        /// FORM DATA/POST DATA/REQUEST BODY
        /// {
        /// TeacherFname: "Jessica"
        /// TeacherLname: "Morris"
        /// EmployeeNumber: "T389"
        /// HireDate: "2012-06-04"
        /// Salary: "48.62"
        /// }
        /// </example>

        [HttpPost]
        //POST : /Teacher/Update/{id}
        public ActionResult Update(int id, string TeacherFname, string TeacherLname, DateTime HireDate, string EmployeeNumber, double Salary)
        {

            Teacher TeacherInfo = new Teacher();
            TeacherInfo.TeacherFname = TeacherFname;
            TeacherInfo.TeacherLname = TeacherLname;
            TeacherInfo.HireDate = HireDate;
            TeacherInfo.EmployeeNumber = EmployeeNumber;
            TeacherInfo.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, TeacherInfo);


            return RedirectToAction("Show/" + id); 

        }
    }
}