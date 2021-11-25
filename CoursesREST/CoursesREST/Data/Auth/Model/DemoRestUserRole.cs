using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Data.Auth.Model
{
	public static class DemoRestUserRole
	{
		public const string Admin = nameof(Admin);
		public const string User = nameof(User);
		public const string PremiumUser = nameof(PremiumUser);
		public static readonly IReadOnlyCollection<string> All = new[] { Admin, User, PremiumUser };

	}
}
