using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManagementSystem.Models
{
	public class Attendance
	{
        [Key]
        public int AttendanceId { get; set; }

        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime Date { get; set; }

        public bool IsPresent { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }
}