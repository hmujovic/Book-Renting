using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore_API.Models
{
	public class Book
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public int Year { get; set; }
		public string Summary { get; set; }
		public int Quantity { get; set; }
	}
}
