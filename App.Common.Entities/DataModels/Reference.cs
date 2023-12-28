using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.DataModels
{
    public class Reference
    {
        [Key]
        public int ReferenceId { get; set; }
        public string? ReferenceName { get; set; }
        public string? Position { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public int CVId { get; set; }
        [ForeignKey("CVId")]
        public virtual UserCv UserCv { get; set; }
    }
}
