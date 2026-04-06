using StudentManagementSystem.Models;
using System.Linq;
using System.Web.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class EnrollmentController : Controller
    {

        StudentContext db = new StudentContext();

        public ActionResult Create()
        {
            if (Session["User"] == null || Session["Role"].ToString() != "Admin")
                return RedirectToAction("Login", "Account");

            ViewBag.Students = new SelectList(db.Students, "StudentId", "Name");
            ViewBag.Courses = new SelectList(db.Courses, "CourseId", "CourseName");
            return View();
        }
        
        
        [HttpPost]
        public ActionResult Create(Enrollment enrollment)
        {
            db.Enrollments.Add(enrollment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            var data = db.Enrollments
                .Include("Student")
                .Include("Course")
                .ToList();

            return View(data);
        }
    }
}
