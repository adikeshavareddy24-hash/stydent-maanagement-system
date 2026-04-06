using StudentManagementSystem.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StudentManagementSystem.Controllers
{
    [AuthFilter]
    public class DashboardController : Controller
    {
        StudentContext db = new StudentContext();

        // GET: Dashboard
        
        public ActionResult Index()
        {
            //if (Session["Role"] == null )
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            if (Session["Role"]?.ToString() == "Student")
            {
                int sid = Convert.ToInt32(Session["StudentId"]);
                var student = db.Students.Find(sid);

                if (!student.IsProfileCompleted)
                {
                    return RedirectToAction("CompleteProfile", "Student");
                }
            }



            // Total students
            ViewBag.TotalStudents = db.Students.Count();

            // Total courses
            ViewBag.TotalCourses = db.Courses.Count();

            // Current logged in user
            ViewBag.Username = Session["User"].ToString();

            // Database name (for debugging)
            ViewBag.DbName = db.Database.Connection.Database;

            return View();
        }
    }
}
