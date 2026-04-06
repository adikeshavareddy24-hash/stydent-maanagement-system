    using StudentManagementSystem.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    namespace StudentManagementSystem.Controllers
    {
        [AuthFilter]
        public class AttendanceController : Controller
        {
            StudentContext db = new StudentContext();

            public ActionResult Mark()
            {
                if (Session["User"] == null || Session["Role"].ToString() != "Admin")
                    return RedirectToAction("Login", "Account");

                ViewBag.Students = db.Students.ToList();
                ViewBag.Courses = db.Courses.ToList();
            return View();
            }
            [AuthFilter]
            [AdminOnly]
        [HttpPost]
        public ActionResult Mark(int studentId, int courseId, bool isPresent, DateTime attendanceDate)
        {


            bool alreadyExists = db.Attendances.Any(att =>
                att.StudentId == studentId &&
                att.CourseId == courseId &&
                DbFunctions.TruncateTime(att.Date) == attendanceDate);

            if (alreadyExists)
            {
                TempData["Error"] = "Attendance already marked today!";
                return RedirectToAction("Mark");
            }

            Attendance attendanceRecord = new Attendance
            {
                StudentId = studentId,
                CourseId = courseId,
                Date = attendanceDate,
                IsPresent = isPresent
            };

            db.Attendances.Add(attendanceRecord);
            db.SaveChanges();

            return RedirectToAction("Mark");
        }


        [AuthFilter]
        public ActionResult Report()
        {
            if (Session["Role"]?.ToString() == "Student")
            {
                int sid = Convert.ToInt32(Session["StudentId"]);

                var attendance = db.Attendances
                    .Where(att => att.StudentId == sid)
                    .Include("Student")
                    .Include("Course")
                    .ToList();
                // 🔹 Overall Percentage
                int totalClasses = attendance.Count();
                int totalPresent = attendance.Count(x => x.IsPresent);

                double overallPercentage = totalClasses == 0 ? 0 :
                    (double)totalPresent * 100 / totalClasses;

                ViewBag.OverallPercentage = overallPercentage;
                // Course-wise percentage
                var courseWise = attendance
      .GroupBy(att => att.Course.CourseName)
      .Select(g => new CourseAttendanceVM
      {
          Course = g.Key,
          Total = g.Count(),
          Present = g.Count(x => x.IsPresent),
          Percentage = g.Count() == 0 ? 0 :
              (double)g.Count(x => x.IsPresent) * 100 / g.Count()
      })
      .ToList();

                ViewBag.CourseWise = courseWise;


                return View(attendance);
            }

            // Admin
            var all = db.Attendances
                .Include("Student")
                .Include("Course")
                .ToList();

            return View(all);
        }

        [AuthFilter]
            [AdminOnly]
            public ActionResult StudentAttendance(int id)
            {
            var attendance = db.Attendances
               .Where(att => att.StudentId == id)
               .Include("Student")
               .Include("Course")   // 👈 ADD THIS
               .ToList();


            return View(attendance);
            }



        }
    }