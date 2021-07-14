using BookStore_API.Contracts;
using BookStore_API.Data;
using BookStore_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_API.Services
{
	public class BookRepository : IBookRepository
	{
		private readonly DataContext _db;

		public BookRepository(DataContext db)
		{
			_db = db;
		}

		public async Task<bool> Create(Book entity)
		{
			await _db.Books.AddAsync(entity);
			return await Save();
		}

		public async Task<bool> Delete(Book entity)
		{
			_db.Books.Remove(entity);
			return await Save();
		}

		public async Task<IList<Book>> FindAll()
		{
			var books = await _db.Books
				.ToListAsync();
			return books;
		}

		public async Task<Book> FindById(int id)
		{
			var book = await _db.Books
				.FirstOrDefaultAsync(q => q.Id == id);
			return book;
		}

		public async Task<bool> isExists(int id)
		{
			var isExists = await _db.Books.AnyAsync(x => x.Id == id);
			return isExists;
		}

		public async Task<bool> Save()
		{
			var changes = await _db.SaveChangesAsync();
			return changes > 0;
		}

		public async Task<bool> Update(Book entity)
		{
			_db.Books.Update(entity);
			return await Save();
		}
	}
}
