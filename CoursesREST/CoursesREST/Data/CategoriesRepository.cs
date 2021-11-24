using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Data
{
	public interface ICategoriesRepository
	{
		Task Delete(Category category);
		Task<Category> Get(int id);
		Task<IEnumerable<Category>> GetAll();
		Task Insert(Category category);
		Task Update(Category category);
	}

	public class CategoriesRepository : ICategoriesRepository
	{
		private readonly Context _context;
		public CategoriesRepository(Context context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Category>> GetAll()
		{
			return await _context.Categories.ToListAsync();
		}

		public async Task<Category> Get(int id)
		{
			return await _context.Categories.FirstOrDefaultAsync(o => o.Id == id);
		}

		public async Task Insert(Category category)
		{
			_context.Categories.Add(category);
			await _context.SaveChangesAsync();
		}
		public async Task Update(Category category)
		{
			_context.Categories.Update(category);
			await _context.SaveChangesAsync();
		}
		public async Task Delete(Category category)
		{
			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();
		}
	}
}
