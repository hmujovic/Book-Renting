using BookStore_API.Models;
using BookStore_API.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookStore_API.Data
{
	public static class SeedData
	{
		public async static Task Seed(UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager)
		{
			await SeedRoles(roleManager);
			await SeedUsers(userManager);
		}
		private async static Task SeedUsers(UserManager<IdentityUser> userManager)
		{
			if (await userManager.FindByEmailAsync("librarian@bookstore.com") == null)
			{
				var user = new IdentityUser
				{
					UserName = "librarian@bookstore.com",
					Email = "librarian@bookstore.com"
				};

				var result = await userManager.CreateAsync(user, "Pa$$sw0rd");
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, "Librarian");
				}
			}
		}
		private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			if (!await roleManager.RoleExistsAsync("Librarian"))
			{
				var role = new IdentityRole
				{
					Name = "Librarian"
				};
				await roleManager.CreateAsync(role);
			}

			if (!await roleManager.RoleExistsAsync("Member"))
			{
				var role = new IdentityRole
				{
					Name = "Member"
				};
				await roleManager.CreateAsync(role);
			}
		}
	}
}