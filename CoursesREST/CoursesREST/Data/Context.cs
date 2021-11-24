using CoursesREST.Data.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Data
{
	public class Context : IdentityDbContext<DemoRestUser>
	{
		public DbSet<Category> Categories { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Comment> Comments { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=CoursesTest");
			//optionsBuilder.UseSqlServer("Data Source =tcp:coursesrestdbserver.database.windows.net,1433;Initial Catalog = CoursesREST_db; User Id = coursesadmin@coursesrestdbserver; Password = Coursesfornoobs123");
		}
	}
}
