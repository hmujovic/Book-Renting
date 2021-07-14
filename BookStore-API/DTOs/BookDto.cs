using System.ComponentModel.DataAnnotations;

namespace BookStore_API.DTOs
{
	public class BookDto
	{
		[Required]
		public string Title { get; set; }
		[Required]
		public int Year { get; set; }
		[Required]
		[StringLength(500)]
		public string Summary { get; set; }
		[Required]
		public string Author { get; set; }
		[Required]
		public int Quantity { get; set; }
	}
}
