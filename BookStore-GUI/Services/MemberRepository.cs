using Blazored.LocalStorage;
using BookStore_GUI.Contracts;
using BookStore_GUI.Models;
using System.Net.Http;

namespace BookStore_GUI.Services
{
	public class MemberRepository : BaseRepository<Member>, IMemberRepository
	{
		private readonly IHttpClientFactory _client;
		private readonly ILocalStorageService _localStorage;

		public MemberRepository(IHttpClientFactory client,
			ILocalStorageService localStorage) : base(client, localStorage)
		{
			_client = client;
			_localStorage = localStorage;
		}
	}
}
