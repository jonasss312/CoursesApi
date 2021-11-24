using CoursesREST.Data.Auth;
using CoursesREST.Data.Auth.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Data
{
	public class Course : IUserOwnedResource
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public int CategoryId { get; set; }
		public Category Category { get; set; }
		[Required]
		public string UserId { get; set; }

		public DemoRestUser User { get; set; }
	}
}
