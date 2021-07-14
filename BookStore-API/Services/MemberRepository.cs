using BookStore_API.Contracts;
using BookStore_API.Data;
using BookStore_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_API.Services
{
	public class MemberRepository : IMemberRepository
	{
		private readonly DataContext _db;

		public MemberRepository(DataContext db)
		{
			_db = db;
		}

		public async Task<bool> Create(Member entity)
		{
			await _db.Members.AddAsync(entity);
			return await Save();
		}

		public async Task<bool> Delete(Member entity)
		{
			_db.Members.Remove(entity);
			return await Save();
		}

		public async Task<IList<Member>> FindAll()
		{
			var members = await _db.Members
				.ToListAsync();
			return members;
		}

		public async Task<Member> FindById(int id)
		{
			var member = await _db.Members
				.FirstOrDefaultAsync(q => q.Id == id);
			return member;
		}

		public async Task<Member> FindByUsername(string username)
		{
			var member = await _db.Members
				.FirstOrDefaultAsync(q => q.Name == username);
			return member;
		}

		public async Task<bool> isExists(int id)
		{
			var isExists = await _db.Members.AnyAsync(x => x.Id == id);
			return isExists;
		}

		public async Task<bool> Save()
		{
			var changes = await _db.SaveChangesAsync();
			return changes > 0;
		}

		public async Task<bool> Update(Member entity)
		{
			_db.Members.Update(entity);
			return await Save();
		}
	}
}
