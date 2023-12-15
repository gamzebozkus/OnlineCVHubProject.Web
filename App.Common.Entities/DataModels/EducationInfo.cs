using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.DataModels
{
    public class EducationInfo
    {

        [Key]
        public int EducationInfoId { get; set; }
        public string School { get; set; }
        public string Major { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int CVId { get; set; }
        [ForeignKey("CVId")]
        public virtual UserCv UserCv { get; set; }
    }
}
