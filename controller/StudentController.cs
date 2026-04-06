using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        // Database object
        StudentContext db = new StudentContext();
        // GET: Student
        public ActionResult Index(string search, int page = 1)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }


            int pageSize = 5; // records per page

            var students = db.Students.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                students = students.Where(s =>
                    s.Name.Contains(search) ||
                    s.Email.Contains(search));
            }

            int totalRecords = students.Count();

            var studentList = students
                .OrderBy(s => s.StudentId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.Search = search;

            return View(studentList);
        }


        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Student student)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Save student to DB
            student.IsProfileCompleted = false;

            db.Students.Add(student);
            db.SaveChanges(); // Returns number of rows affected
            

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var student = db.Students.Find(id);
            return View(student);
        }
        [HttpPost]
        public ActionResult Edit(Student student)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
                return View(student);

            var existingStudent = db.Students.Find(student.StudentId);
            if (existingStudent == null)
                return HttpNotFound();

            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.Phone = student.Phone;
            existingStudent.DateOfBirth = student.DateOfBirth;

            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult Delete(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CompleteProfile()
        {
            int sid = Convert.ToInt32(Session["StudentId"]);
            var student = db.Students.Find(sid);
            return View(student);
        }
        [HttpPost]
        public ActionResult CompleteProfile(Student model)
        {
            var student = db.Students.Find(model.StudentId);

            student.Email = model.Email;
            student.Phone = model.Phone;
            student.DateOfBirth = model.DateOfBirth;
            student.IsProfileCompleted = true;

            db.SaveChanges();
            return RedirectToAction("Index", "Dashboard");
        }





    }
}