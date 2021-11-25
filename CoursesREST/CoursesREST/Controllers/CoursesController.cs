using CoursesREST.Data;
using CoursesREST.Data.Auth.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace CoursesREST.Controllers
{
	[ApiController]
	[Route("api/categories/{categoryId}/courses")]
	public class CoursesController : ControllerBase
	{
		private readonly ICoursesRepository _coursesRepository;
		private readonly ICategoriesRepository _categoriesRepository;
        private readonly IAuthorizationService _authorizationService;

        public CoursesController(ICoursesRepository coursesRepository, ICategoriesRepository categoriesRepository, IAuthorizationService authorizationService)
		{
			_coursesRepository = coursesRepository;
			_categoriesRepository = categoriesRepository;
			_authorizationService = authorizationService;
		}

		[HttpGet]
		public async Task<IEnumerable<Course>> GetAll(int categoryId)
		{
			return await _coursesRepository.GetAll(categoryId);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Course>> Get(int categoryId, int id)
		{
			var course = await _coursesRepository.Get(categoryId, id);
			if (course == null) return NotFound();
			return Ok(course);
		}

		[HttpPost]
		[Authorize(Roles = DemoRestUserRole.PremiumUser)]
		public async Task<ActionResult<Course>> Insert(int categoryId, CourseDto course)
		{
			var category = await _categoriesRepository.Get(categoryId);
			if (category == null) return NotFound($"Couldn't find a category with id of {categoryId}");

			ClaimsIdentity claimIdentity = User.Identity as ClaimsIdentity;
			var userId = claimIdentity?.FindFirst(CustomClaim.UserId)?.Value;

			Course newCourse = new Course();

			newCourse.Id = course.Id;
			newCourse.Name = course.Name;
			newCourse.Description = course.Description;
			newCourse.CategoryId = categoryId;
			newCourse.UserId = userId;
			newCourse.CategoryId = categoryId;
			newCourse.Category = category;
			await _coursesRepository.Insert(newCourse);

			return Created($"/api/categories/{categoryId}/courses/{course.Id}", course);
		}

		[HttpPatch("{id}")]
		[Authorize(Roles = DemoRestUserRole.PremiumUser)]
		public async Task<ActionResult<Course>> Update(int categoryId, int id, CourseDto course)
		{
			var category = await _categoriesRepository.Get(categoryId);
			if (category == null) return NotFound($"Couldn't find a category with id of {categoryId}");

			var oldCourse = await _coursesRepository.Get(categoryId, id);
			if (oldCourse == null)
				return NotFound();

			var authenticationResult = await _authorizationService.AuthorizeAsync(User, oldCourse, PolicyNames.SameUser);
			if (!authenticationResult.Succeeded)
				return Forbid();

			oldCourse.Name = course.Name;
			oldCourse.Description = course.Description;

			await _coursesRepository.Update(oldCourse);

			return Ok(category);
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = DemoRestUserRole.PremiumUser)]
		public async Task<ActionResult<Course>> Delete(int categoryId, int id)
		{
			var courses = await _coursesRepository.Get(categoryId, id);
			if (courses == null) return NotFound();

			var authenticationResult = await _authorizationService.AuthorizeAsync(User, courses, PolicyNames.SameUser);
			if (!authenticationResult.Succeeded)
				return Forbid();

			await _coursesRepository.Delete(courses);

			return NoContent();
		}
	}
}
