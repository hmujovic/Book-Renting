namespace BookStore_GUI.Static
{
	public static class Endpoints
	{
		public static string BaseUrl = "https://localhost:44370/";

		public static string MembersEndpoint = $"{BaseUrl}api/members/";
		public static string BooksEndpoint = $"{BaseUrl}api/books/";
		public static string RentalEndpoint = $"{BaseUrl}api/rentals/";
		public static string RegisterEndpoint = $"{BaseUrl}api/users/register/";
		public static string LoginEndpoint = $"{BaseUrl}api/users/login/";
	}
}
