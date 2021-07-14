using BookStore_API.Models;
using System.Threading.Tasks;

namespace BookStore_API.Contracts
{
	public interface IMemberRepository : IRepositoryBase<Member>
	{
		Task<Member> FindByUsername(string username);
	}
}
