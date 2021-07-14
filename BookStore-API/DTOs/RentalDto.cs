using BookStore_API.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore_API.DTOs
{
	public class RentalDto
	{
		public int Id { get; set; }
		public string MemberName { get; set; }
		public string BookName { get; set; }
		public int BookId { get; set; }
		public DateTime RentalCreation { get; set; }
		public DateTime RentalDueDate { get; set; }
		public RentalStatus Status { get; set; }
	}

	public class RentalCreateDto
	{
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
		public RentalStatus Status { get; set; } = RentalStatus.Pending;
	}
	public class RentalUpdateDto
	{
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
