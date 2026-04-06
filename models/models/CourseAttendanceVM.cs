using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystem.Models
{
    public class CourseAttendanceVM
    {
        public string Course { get; set; }
        public int Total { get; set; }
        public int Present { get; set; }
        public double Percentage { get; set; }
    }
}