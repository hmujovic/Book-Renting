using Blazored.LocalStorage;
using BookStore_GUI.Contracts;
using BookStore_GUI.Models;
using BookStore_GUI.Providers;
using BookStore_GUI.Static;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_GUI.Services
{
	public class AuthenticationRepository : IAuthenticationRepository
	{
		private readonly IHttpClientFactory _client;
		private readonly ILocalStorageService _localStorage;
		private readonly AuthenticationStateProvider _authenticationStateProvider;

		public AuthenticationRepository(IHttpClientFactory client,
			ILocalStorageService localStorage,
			AuthenticationStateProvider authenticationStateProvider)
		{
			_client = client;
			_localStorage = localStorage;
			_authenticationStateProvider = authenticationStateProvider;
		}

		public async Task<string> ActiveUser()
		{
			var token = await _localStorage.GetItemAsync<string>("authToken");
			var handler = new JwtSecurityTokenHandler();
			var jsonToken = handler.ReadToken(token);
			var tokenS = jsonToken as JwtSecurityToken;

			var username = tokenS.Claims.First(claim => claim.Type == "sub").Value;

			return username.ToString();
		}

		public async Task<bool> Login(LoginModel user)
		{
			var request = new HttpRequestMessage(HttpMethod.Post,
				Endpoints.LoginEndpoint);
			request.Content = new StringContent(JsonConvert.SerializeObject(user),
				Encoding.UTF8, "application/json");

			var client = _client.CreateClient();
			HttpResponseMessage response = await client.SendAsync(request);

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			var content = await response.Content.ReadAsStringAsync();
			var token = JsonConvert.DeserializeObject<TokenResponse>(content);

			//Store Token
			await _localStorage.SetItemAsync("authToken", token.Token);

			//Change auth state of app
			await ((ApiAuthenticationStateProvider)_authenticationStateProvider)
				.LoggedIn();

			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("bearer", token.Token);

			return true;
		}

		public async Task Logout()
		{
			await _localStorage.RemoveItemAsync("authToken");
			((ApiAuthenticationStateProvider)_authenticationStateProvider)
				.LoggedOut();
		}

		public async Task<bool> Register(RegistrationModel user)
		{
			var request = new HttpRequestMessage(HttpMethod.Post,
				Endpoints.RegisterEndpoint);
			request.Content = new StringContent(JsonConvert.SerializeObject(user),
				Encoding.UTF8, "application/json");

			var client = _client.CreateClient();
			HttpResponseMessage response = await client.SendAsync(request);

			return response.IsSuccessStatusCode;
		}
	}
}
