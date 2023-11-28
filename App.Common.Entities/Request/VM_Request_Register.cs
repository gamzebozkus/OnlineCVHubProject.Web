using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.Common.Entities.Request
{
    public class VM_Request_Register
    {

		
		[Display(Name = "First Name")]
		public string? FirstName { get; set; }

		
		[Display(Name = "Last Name")]
		public string? LastName { get; set; }

		[Required(ErrorMessage = "Email alanı zorunludur.")]
		[EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password alanı zorunludur.")]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
}
