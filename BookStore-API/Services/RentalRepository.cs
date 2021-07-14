using BookStore_API.Contracts;
using BookStore_API.Data;
using BookStore_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore_API.Services
{
	public class RentalRepository : IRentalRepository
	{
		private readonly DataContext _db;

		public RentalRepository(DataContext db)
		{
			_db = db;
		}

		public async Task<bool> Create(Rental entity)
		{
			await _db.Rentals.AddAsync(entity);
			return await Save();
		}

		public async Task<bool> Delete(Rental entity)
		{
			_db.Rentals.Remove(entity);
			return await Save();
		}

		public async Task<IList<Rental>> FindAll()
		{
			var Rentals = await _db.Rentals
				.ToListAsync();
			return Rentals;
		}

		public async Task<Rental> FindById(int id)
		{
			var Rental = await _db.Rentals
				.FirstOrDefaultAsync(q => q.Id == id);
			return Rental;
		}

		public async Task<bool> isExists(int id)
		{
			var isExists = await _db.Rentals.AnyAsync(x => x.Id == id);
			return isExists;
		}

		public async Task<bool> Save()
		{
			var changes = await _db.SaveChangesAsync();
			return changes > 0;
		}

		public async Task<bool> Update(Rental entity)
		{
			_db.Rentals.Update(entity);
			return await Save();
		}
	}
}
