using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Data
{
	public interface ICoursesRepository
	{
		Task Delete(Course course);
		Task<Course> Get(int categoryId, int courseId);
		Task<IEnumerable<Course>> GetAll(int categoryId);
		Task Insert(Course course);
		Task Update(Course course);
	}

	public class CoursesRepository : ICoursesRepository
	{
		private readonly Context _context;
		public CoursesRepository(Context context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Course>> GetAll(int categoryId)
		{
			return await _context.Courses.Where(o => o.CategoryId == categoryId).ToListAsync();
		}

		public async Task<Course> Get(int categoryId, int courseId)
		{
			return await _context.Courses.FirstOrDefaultAsync(o => o.Id == courseId && o.CategoryId == categoryId);
		}

		public async Task Insert(Course course)
		{
			_context.Courses.Add(course);
			await _context.SaveChangesAsync();
		}
		public async Task Update(Course course)
		{
			_context.Courses.Update(course);
			await _context.SaveChangesAsync();
		}
		public async Task Delete(Course course)
		{
			_context.Courses.Remove(course);
			await _context.SaveChangesAsync();
		}
	}
}
