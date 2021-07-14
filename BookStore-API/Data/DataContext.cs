using BookStore_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore_API.Data
{
	public class DataContext : IdentityDbContext
	{
		public DbSet<Member> Members { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<Rental> Rentals { get; set; }
		public DataContext(DbContextOptions<DataContext> options)
			: base(options)
		{
		}
	}
}
