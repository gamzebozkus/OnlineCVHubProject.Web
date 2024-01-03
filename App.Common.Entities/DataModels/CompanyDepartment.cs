using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.DataModels
{
    public class CompanyDepartment
    {
        [Key]
        public int Id { get; set; }

        public string? DepartmentName { get; set; }

        public string CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public virtual tblUser tblUser { get; set; }

    }
}
