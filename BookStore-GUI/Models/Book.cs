using System.ComponentModel.DataAnnotations;

namespace BookStore_GUI.Models
{
	public class Book
	{
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public int Year { get; set; }
		[Required]
		[StringLength(150)]
		public string Summary { get; set; }
		[Required]
		public string Author { get; set; }
		[Required]
		public int Quantity { get; set; }
	}
}
