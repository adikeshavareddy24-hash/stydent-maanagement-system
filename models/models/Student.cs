using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManagementSystem.Models
{
	public class Student
	{
        // Primary Key
        
        public int StudentId { get; set; }

        // Student Name
        [Required]
        public string Name { get; set; }

        // Email Address
        [EmailAddress]
        public string Email { get; set; }

        // Phone Number
        public string Phone { get; set; }

        // Date of Birth
        public DateTime? DateOfBirth { get; set; }
        public bool IsProfileCompleted { get; set; }


    }
}