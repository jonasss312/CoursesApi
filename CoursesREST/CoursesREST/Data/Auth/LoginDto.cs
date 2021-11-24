using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Data.Auth
{
	public record LoginDto(string UserName, string Password);
}
