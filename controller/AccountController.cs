using StudentManagementSystem.Models;
using StudentManagementSystem.Helpers;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;




namespace StudentManagementSystem.Controllers
{

    public class AccountController : Controller
    {
        // Database context
        StudentContext db = new StudentContext();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
public ActionResult Login(string username, string password)
{
    string hashedPassword = PasswordHelper.HashPassword(password);

    var user = db.Users.FirstOrDefault(u =>
        u.Username == username &&
        u.Password == hashedPassword);

    if (user != null)
    {
        Session["User"] = user.Username;
        Session["Role"] = user.Role;
        Session["StudentId"] = user.StudentId;

        return RedirectToAction("Index", "Dashboard");
    }

    ViewBag.Error = "Invalid username or password";
    return View();
}


        // GET: Forgot Password
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Forgot Password
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var user = (from u in db.Users
                        join s in db.Students on u.StudentId equals s.StudentId
                        where s.Email == email
                        select u).FirstOrDefault();

            if (user == null)
            {
                ViewBag.Error = "Email not found";
                return View();
            }

            string token = Guid.NewGuid().ToString();

            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.Now.AddMinutes(30);
            db.SaveChanges();

            string resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { token = token },
                Request.Url.Scheme
            );

            // 🔥 REAL MAIL SEND
            SendResetEmail(email, resetLink);

            ViewBag.Message = "Password reset link sent to your email";
            return View();
        }


        // GET: Reset Password
        public ActionResult ResetPassword(string token)
{
    var user = db.Users.FirstOrDefault(u =>
        u.ResetToken == token &&
        u.ResetTokenExpiry > DateTime.Now);

    if (user == null)
        return Content("Invalid or expired reset link");

    ViewBag.Token = token;
    return View();
}

// POST: Reset Password
[HttpPost]
public ActionResult ResetPassword(string token, string newPassword)
{
    var user = db.Users.FirstOrDefault(u =>
        u.ResetToken == token &&
        u.ResetTokenExpiry > DateTime.Now);

    if (user == null)
        return Content("Invalid or expired reset link");

    user.Password = PasswordHelper.HashPassword(newPassword);
    user.ResetToken = null;
    user.ResetTokenExpiry = null;

    db.SaveChanges();

    return RedirectToAction("Login");
}


        //https://localhost:7027/api/Users

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login", "Account");
        }
        private void SendResetEmail(string toEmail, string resetLink)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("mahesharigela2026@gmail.com");
            mail.To.Add(toEmail);
            mail.Subject = "Reset Password";

            mail.IsBodyHtml = true;

            mail.Body = $@"
<h3>Password Reset Request</h3>
<p>Click the button below to reset your password:</p>
<a href='{resetLink}' 
   style='background-color:#28a745;
          color:white;
          padding:10px 15px;
          text-decoration:none;
          border-radius:5px;'>
   Reset Password
</a>
";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("mahesharigela2026@gmail.com", "jvhi rard yyxl xarp");
            smtp.EnableSsl = true;

            smtp.Send(mail);

        }


        public ActionResult Register(int id)
        {
            ViewBag.StudentId = id;
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            user.Role = "Student";

            user.Password = PasswordHelper.HashPassword(user.Password);

            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index", "Student");

        }
        public ActionResult AdminRegister()
        {
            // 🔒 If admin already exists, block
            if (db.Users.Any(u => u.Role == "Admin"))
            {
                return RedirectToAction("Login");
            }

            return View();
        }
        [HttpPost]
        public ActionResult AdminRegister(User user)
        {
            // 🔒 Safety check again
            if (db.Users.Any(u => u.Role == "Admin"))
            {
                return RedirectToAction("Login");
            }

            user.Role = "Admin";
            user.StudentId = null;

            // 🔐 Hash password
            user.Password = PasswordHelper.HashPassword(user.Password);

            db.Users.Add(user);
            db.SaveChanges();

            return RedirectToAction("Login");
        }

    }
}
