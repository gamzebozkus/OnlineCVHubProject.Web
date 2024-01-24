using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.Request
{
    public class CompanySavedCvViewModel
    {
        public int Id { get; set; }
            public string CvName { get; set; }
            public string CvTitle { get; set; }
            public int CvId { get; set; }
           
            public string CvMail { get; set; }
            public string Notes { get; set; }
            public bool? Status { get; set; }
        // Toplantı tarihi
        public DateTime? MeetingDate { get; set; }

        // Toplantı saati
        public TimeSpan? MeetingTime { get; set; }

        // Toplantı konusu
        public string? MeetingSubject { get; set; }


    }
}
