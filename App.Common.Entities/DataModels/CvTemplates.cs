using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.DataModels
{
    public class CvTemplates
    {
        [Key]
        public int Id {get; set;}
        public int TemplateId { get; set; }


        [ForeignKey(nameof(CVId))]
        public int CVId { get; set; }

    }
}
