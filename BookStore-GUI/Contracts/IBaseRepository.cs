using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_GUI.Contracts
{
	public interface IBaseRepository<T> where T : class
	{
		Task<T> Get(string url, int id);
		Task<IList<T>> Get(string url);
		Task<T> Get(string url, string username);
		Task<bool> Create(string url, T obj);
		Task<bool> Update(string url, T obj, int id);
		Task<bool> Delete(string url, int id);
	}
}
