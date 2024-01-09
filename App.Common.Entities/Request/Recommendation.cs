﻿using App.Common.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.Common.Entities.Request
{
    public class Recommendation
    {
        [Required(ErrorMessage = "Şirket adı alanı zorunludur.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [Display(Name = "Email")]
        public string CompanyMail { get; set; }

        [Required(ErrorMessage = "Password alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string CompanyPassword { get; set; }

        public bool RememberMe { get; set; }
        public List<Recommendation> Sonuclar { get; set; }

        public List<string> DepartmentName { get; set; }
        //public List<string> Sonuclar { get; set; } = new List<string>();
        public string? PersonalityTraits { get; set; }
        public virtual List<Skill> Skills { get; set; }
        public double Similarity { get; set; }
        public virtual List<Language> Languages { get; set; }
        public string CvId { get; set; }

        [ForeignKey(nameof(CvId))]
        public virtual CvPool CvPool { get; set; }
    }
}
