using CoursesREST.Data.Auth;
using CoursesREST.Data.Auth.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Data
{
	public class DatabaseSeeder
	{
		private UserManager<DemoRestUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;

		public DatabaseSeeder(UserManager<DemoRestUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;

		}

		public async Task SendAsync()
		{
			foreach (var role in DemoRestUserRole.All)
			{
				var roleExist = await _roleManager.RoleExistsAsync(role);
				if (!roleExist)
				{
					await _roleManager.CreateAsync(new IdentityRole(role));
				}
			}

			var newAdminUser = new DemoRestUser
			{
				UserName = "admin",
				Email = "admin@mail.com"
			};

			var existingAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);
			if (existingAdminUser == null)
			{
				var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "VerySafePassword1!");
				if (createAdminUserResult.Succeeded)
				{
					await _userManager.AddToRolesAsync(newAdminUser, DemoRestUserRole.All);
				}
			}

			newAdminUser = new DemoRestUser
			{
				UserName = "renewAdmin",
				Email = "admin@mail.com"
			};

			existingAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);
			if (existingAdminUser == null)
			{
				var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "VerySafePassword1!");
				if (createAdminUserResult.Succeeded)
				{
					await _userManager.AddToRolesAsync(newAdminUser, DemoRestUserRole.All);
				}
			}

			var newPremiumUser = new DemoRestUser
			{
				UserName = "premiumUser",
				Email = "admin@mail.com"
			};

			var existingPremiumUser = await _userManager.FindByNameAsync(newPremiumUser.UserName);
			if (existingPremiumUser == null)
			{
				var createPremiumUserResult = await _userManager.CreateAsync(newPremiumUser, "VerySafePassword1!");
				if (createPremiumUserResult.Succeeded)
				{
					await _userManager.AddToRoleAsync(newPremiumUser, DemoRestUserRole.User);
					await _userManager.AddToRoleAsync(newPremiumUser, DemoRestUserRole.PremiumUser);
				}
			}

			newPremiumUser = new DemoRestUser
			{
				UserName = "premiumUser2",
				Email = "admin@mail.com"
			};

			existingPremiumUser = await _userManager.FindByNameAsync(newPremiumUser.UserName);
			if (existingPremiumUser == null)
			{
				var createPremiumUserResult = await _userManager.CreateAsync(newPremiumUser, "VerySafePassword1!");
				if (createPremiumUserResult.Succeeded)
				{
					await _userManager.AddToRoleAsync(newPremiumUser, DemoRestUserRole.User);
					await _userManager.AddToRoleAsync(newPremiumUser, DemoRestUserRole.PremiumUser);
				}
			}
		}
	}
}
