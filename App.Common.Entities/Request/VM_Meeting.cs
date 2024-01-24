using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.Request
{
    public class VM_Meeting
    {

        public string? MeetingSubject { get; set; } // Toplantı Konusu
        public DateTime? MeetingDate { get; set; } // Toplantı Tarihi
        public TimeSpan? MeetingTime { get; set; } // Toplantı Saati
        public string CvNameSurname { get; set; }
    }
}
