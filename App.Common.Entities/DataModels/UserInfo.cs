using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Entities.DataModels
{
    public class UserInfo
    {
        public int UserInfoId { get; set; }

        // AspNetUsers tablosundaki Id ile ilişkilendirme
        [ForeignKey("AspNetUser")]
        public string UserId { get; set; } // ASP.NET Identity'deki Id tipinde olabilir

        public tblUser? AspNetUser { get; set; } // ASP.NET Identity User tipi

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; } = "Belirtilmemiş";


    }
}
