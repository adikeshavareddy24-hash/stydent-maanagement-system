using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManagementSystem.Models
{
	public class Course
    {
        public int CourseId { get; set; }

        [Required]
        public string CourseName { get; set; }

        public string Description { get; set; }

        public int DurationWeeks { get; set; }
    }
}