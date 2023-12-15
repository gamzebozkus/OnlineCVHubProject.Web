using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.DataModels
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Institution { get; set; }
        public DateTime CompletionDate { get; set; }

        public int CVId { get; set; }
        [ForeignKey("CVId")]
        public virtual UserCv UserCv { get; set; }
    }
}
