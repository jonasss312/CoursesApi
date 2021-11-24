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
	public class Comment: IUserOwnedResource
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Text { get; set; }
		public DateTime CreationTime { get; set; }

		public int CourseId { get; set; }
		public Course Course { get; set; }

		public string UserId { get; set; }

		public DemoRestUser User { get; set; }
	}
}
