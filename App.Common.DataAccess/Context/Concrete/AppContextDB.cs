using App.Common.Entities.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.DataAccess.Context.Concrete
{
	public class AppContextDB:IdentityDbContext<tblUser>
	{

		public AppContextDB(DbContextOptions<AppContextDB> options) : base(options)
		{
		}

		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public virtual DbSet<tblUser> Users { get; set; }
        public virtual DbSet<UserCv> UserCVs { get; set; }
        public virtual DbSet<EducationInfo> EducationInfos { get; set; }
        public virtual DbSet<Experience> Experiences { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Hobby> Hobbies { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Reference> References { get; set; }
        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<UserInfo> UserInfos { get; set; }
        public virtual DbSet<CompanyInfo> CompanyInfos { get; set; }
        public virtual DbSet<CvTemplates> CvTemplatess { get; set; }

    }
}
