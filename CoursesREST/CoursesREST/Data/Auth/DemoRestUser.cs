using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Data.Auth
{
	public class DemoRestUser : IdentityUser
	{
		[PersonalData]
		public string AdditionalInfo { get; set; }
	}
}
