using StudentManagementSystem.Models;
using System.Linq;
using System.Web.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class CourseController : Controller
    {
        StudentContext db = new StudentContext();

        // GET: Course
        public ActionResult Index()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }


            var courses = db.Courses.ToList();
            return View(courses);
        }

        // GET: Create
        public ActionResult Create()
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
                return RedirectToAction("Index");

            return View(new Course()); // pass empty model
        }

        // POST: Create
        [HttpPost]
        public ActionResult Create(Course course)
        {
            if (!ModelState.IsValid)
                return View(course);

            db.Courses.Add(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Edit
        public ActionResult Edit(int id)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
                return RedirectToAction("Index");

            var course = db.Courses.Find(id);
            if (course == null) return HttpNotFound();

            return View(course);
        }

        // POST: Edit
        [HttpPost]
        public ActionResult Edit(Course course)
        {
            if (!ModelState.IsValid)
                return View(course);

            var existing = db.Courses.Find(course.CourseId);
            if (existing == null) return HttpNotFound();

            existing.CourseName = course.CourseName;
            existing.Description = course.Description;
            existing.DurationWeeks = course.DurationWeeks;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Delete
        public ActionResult Delete(int id)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
                return RedirectToAction("Index");

            var course = db.Courses.Find(id);
            if (course != null)
            {
                db.Courses.Remove(course);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
