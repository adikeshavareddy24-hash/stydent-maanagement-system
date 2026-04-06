using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystem.Models
{
	public class User
	{
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // hashing
        public string Role { get; set; } // Admin / Student
        public int? StudentId { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

    }
}