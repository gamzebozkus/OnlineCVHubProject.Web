using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.Common.Entities.Request
{
	public class VM_Request_CompanyRegister
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

        public string? DepartmentName { get; set; }

    }
}
