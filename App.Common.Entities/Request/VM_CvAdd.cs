using App.Common.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.Request
{
    public class VM_CvAdd 
    {
        public VM_CvAdd()
        {
            
            EducationInfos = new List<EducationInfo> { new EducationInfo() };
            Experiences=new List<Experience> { new Experience() };
            Skills=new List<Skill> { new Skill() };
            Hobbies=new List<Hobby> { new Hobby() };
            Languages=new List<Language> { new Language() };
        }
        public string? CvNameSurname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNum { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string? State { get; set; }
        public string Image { get; set; }
        public string? Tags { get; set; }
        public string? PersonalityTraits { get; set; }
        public bool? IsConfirmed { get; set; }
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual tblUser tblUser { get; set; }

        public virtual List<EducationInfo> EducationInfos { get; set; }
        public virtual List<Experience> Experiences { get; set; }
        public virtual List<Skill> Skills { get; set; }
        public virtual List<Hobby> Hobbies { get; set; }
        public virtual List<Language> Languages { get; set; }
        public virtual List<Course> Courses { get; set; }
        public virtual List<Reference> References { get; set; }
    }
}
