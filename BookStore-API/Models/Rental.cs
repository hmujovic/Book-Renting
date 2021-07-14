using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore_API.Models
{
	public enum RentalStatus
	{
		Pending,
		Rented,
		Declined,
		Returned
	}

	public class Rental
	{
		public int Id { get; set; }
		public string MemberName { get; set; }
		public string BookName { get; set; }
		public int BookId { get; set; }
		public DateTime RentalCreation { get; set; }
		public DateTime RentalDueDate { get; set; }
		public RentalStatus Status { get; set; }
	}
}
