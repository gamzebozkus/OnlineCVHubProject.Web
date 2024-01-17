using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.DataModels
{
    public class CompanySavedCv
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserCv")]
        public int CvId { get; set; }

        public bool? Gorusme { get; set; }

        public string? Notes { get; set; }

        // Toplantı tarihi
        public DateTime? MeetingDate { get; set; }

        // Toplantı saati
        public TimeSpan? MeetingTime { get; set; }

        // Toplantı konusu
        public string? MeetingSubject { get; set; }

        [ForeignKey("CompanyDepartment")]
        public int? DepartmentId { get; set; }

        [ForeignKey("AspNetUsers")]
        public string CompanyId { get; set; }

        public DateTime? GorusmeTarihi { get; set; } // Görüşme tarihi

        public virtual UserCv UserCv { get; set; }
        public virtual CompanyDepartment CompanyDepartment { get; set; }
    }

}
