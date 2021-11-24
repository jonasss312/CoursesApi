using CoursesREST.Data;
using CoursesREST.Data.Auth;
using CoursesREST.Data.Auth.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Controllers
{
	[ApiController]
	[Route("api/categories/{categoryId}/courses/{courseId}/comments")]
	public class CommentsController : ControllerBase
	{
		private readonly ICoursesRepository _coursesRepository;
		private readonly IAuthorizationService _authorizationService;
        private readonly ICommentsRepository _commentsRepository;
		public CommentsController(ICommentsRepository commentsRepository, ICoursesRepository coursesRepository, IAuthorizationService authorizationService)
		{
			_commentsRepository = commentsRepository;
			_coursesRepository = coursesRepository;
			_authorizationService = authorizationService;
		}

		[HttpGet]
		public async Task<IEnumerable<Comment>> GetAll(int courseId)
		{
			return await _commentsRepository.GetAll(courseId);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Comment>> Get(int courseId, int id)
		{
			var course = await _commentsRepository.Get(courseId, id);
			if (course == null) return NotFound();
			return Ok(course);
		}

		[HttpPost]
		[Authorize(Roles = DemoRestUserRole.User)]
		public async Task<ActionResult<Comment>> Insert(int categoryId, int courseId, Comment comment)
		{
			var category = await _coursesRepository.Get(categoryId, courseId);
			if (category == null) return NotFound($"Couldn't find a course with id of {courseId}");

			var authorizationResult = await _authorizationService.AuthorizeAsync(User, comment, "User");
			
			if (!authorizationResult.Succeeded)
			{
				return Forbid();
			}
            else 
			{
				string userId = User.FindFirst(CustomClaim.UserId)?.Value;
				comment.CourseId= courseId;
				comment.UserId = userId;
				await _commentsRepository.Insert(comment);

				return Created($"/api/categories/{categoryId}/courses/{courseId}/courses/{comment.Id}", comment);
			}
		}

		[HttpPatch("{id}")]
		public async Task<ActionResult<Comment>> Update(int categoryId, int courseId, int id, Comment comment)
		{
			var category = await _coursesRepository.Get(categoryId, courseId);
			if (category == null) return NotFound($"Couldn't find a course with id of {courseId}");

			var oldComment = await _commentsRepository.Get(courseId, id);
			if (oldComment == null)
				return NotFound();

			oldComment.User = comment.User;
			oldComment.Text = comment.Text;
			oldComment.CreationTime = DateTime.Now;

			await _commentsRepository.Update(oldComment);

			return Ok(oldComment);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<Comment>> Delete(int courseId, int id)
		{
			var comment = await _commentsRepository.Get(courseId, id);
			if (comment == null) return NotFound();

			await _commentsRepository.Delete(comment);

			return NoContent();
		}

	}
}
