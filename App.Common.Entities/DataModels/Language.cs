using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.DataModels
{
    public class Language
    {
        [Key]
        public int LanguageId { get; set; }
        public string? LanguageName { get; set; }
        public string? LanguageLevel { get; set; }

        public int CVId { get; set; }
        [ForeignKey("CVId")]
        public virtual UserCv UserCv { get; set; }
    }
}
