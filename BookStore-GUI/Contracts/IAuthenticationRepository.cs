using BookStore_GUI.Models;
using System.Threading.Tasks;

namespace BookStore_GUI.Contracts
{
	public interface IAuthenticationRepository
	{
		public Task<bool> Register(RegistrationModel user);
		public Task<bool> Login(LoginModel user);
		public Task Logout();
		public Task<string> ActiveUser();
	}
}
