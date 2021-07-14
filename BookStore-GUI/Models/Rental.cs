using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore_GUI.Models
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
		[Required]
		public string MemberName { get; set; }
		[Required]
		public string BookName { get; set; }
		[Required]
		public int BookId { get; set; }
		[Required]
		public DateTime RentalCreation { get; set; }
		public DateTime RentalDueDate { get; set; }
		[Required]
		public RentalStatus Status { get; set; }
	}
}
