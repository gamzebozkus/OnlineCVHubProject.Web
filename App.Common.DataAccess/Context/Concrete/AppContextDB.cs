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
		public AppContextDB(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		public virtual DbSet<tblUser> Users { get; set; }
		
	}
}
