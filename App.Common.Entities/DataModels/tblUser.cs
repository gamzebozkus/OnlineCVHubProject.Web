using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.DataModels
{
	[Table("AspNetUsers")]
	public class tblUser:IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? BirthDate { get; set; }
		public string Gender { get; set; } = "Belirtilmemiş";
	}
}
