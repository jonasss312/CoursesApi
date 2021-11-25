using CoursesREST.Data;
using CoursesREST.Data.Auth;
using CoursesREST.Data.Auth.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
		public async Task<ActionResult<Comment>> Insert(int categoryId, int courseId, CommentDto comment)
		{
			var course = await _coursesRepository.Get(categoryId, courseId);
			if (course == null) return NotFound($"Couldn't find a course with id of {courseId}");

			ClaimsIdentity claimIdentity = User.Identity as ClaimsIdentity;
			string userId = User.FindFirst(CustomClaim.UserId)?.Value;
			string userName = User.Identity.Name;

			Comment newComment = new Comment();
			newComment.Course = course;
			newComment.CourseId = courseId;
			newComment.UserId = userId;
			newComment.Text = comment.Text;
			newComment.CreationTime = DateTime.Now;
			newComment.UserName = userName;
			await _commentsRepository.Insert(newComment);

			return Created($"/api/categories/{categoryId}/courses/{courseId}/courses/{newComment.Id}", newComment);
		}

		[HttpPatch("{id}")]
		[Authorize(Roles = DemoRestUserRole.User)]
		public async Task<ActionResult<Comment>> Update(int categoryId, int courseId, int id, CommentDto comment)
		{
			var category = await _coursesRepository.Get(categoryId, courseId);
			if (category == null) return NotFound($"Couldn't find a course with id of {courseId}");

			var oldComment = await _commentsRepository.Get(courseId, id);
			if (oldComment == null)
				return NotFound();

			var authorizationResult = await _authorizationService.AuthorizeAsync(User, oldComment, PolicyNames.SameUser);

			if (!authorizationResult.Succeeded)
				return Forbid();

			oldComment.Text = comment.Text;
			oldComment.CreationTime = DateTime.Now;

			await _commentsRepository.Update(oldComment);

			return Ok(oldComment);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = DemoRestUserRole.User)]
		public async Task<ActionResult<Comment>> Delete(int courseId, int id)
		{
			var comment = await _commentsRepository.Get(courseId, id);
			if (comment == null) return NotFound();

			var authorizationResult = await _authorizationService.AuthorizeAsync(User, comment, PolicyNames.SameUser);

			if (!authorizationResult.Succeeded)
				return Forbid();

			await _commentsRepository.Delete(comment);

			return NoContent();
		}

	}
}
