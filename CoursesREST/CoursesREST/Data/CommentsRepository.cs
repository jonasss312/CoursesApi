using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Data
{
	public interface ICommentsRepository
	{
		Task Delete(Comment comment);
		Task<Comment> Get(int courseId, int commentId);
		Task<IEnumerable<Comment>> GetAll(int courseId);
		Task Insert(Comment comment);
		Task Update(Comment comment);
	}

	public class CommentsRepository : ICommentsRepository
	{
		private readonly Context _context;
		public CommentsRepository(Context context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Comment>> GetAll(int courseId)
		{
			return await _context.Comments.Where(o => o.CourseId == courseId).ToListAsync();
		}

		public async Task<Comment> Get(int courseId, int commentId)
		{
			return await _context.Comments.FirstOrDefaultAsync(o => o.Id == commentId && o.CourseId == courseId);
		}

		public async Task Insert(Comment comment)
		{
			comment.CreationTime = DateTime.Now;
			_context.Comments.Add(comment);
			await _context.SaveChangesAsync();
		}
		public async Task Update(Comment comment)
		{
			comment.CreationTime = DateTime.Now;
			_context.Comments.Update(comment);
			await _context.SaveChangesAsync();
		}
		public async Task Delete(Comment comment)
		{
			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync();
		}
	}
}
