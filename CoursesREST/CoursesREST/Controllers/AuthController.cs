using AutoMapper;
using CoursesREST.Data.Auth;
using CoursesREST.Data.Auth.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Controllers
{
	[ApiController]
	[AllowAnonymous]
	[Route("api")]
	public class AuthController : ControllerBase
	{
		private UserManager<DemoRestUser> _userManager;
		private IMapper _mapper;
		private ITokenManager _tokenManager;

		public AuthController(UserManager<DemoRestUser> userManager, IMapper mapper, ITokenManager tokenManager)
		{
			_userManager = userManager;
			_mapper = mapper;
			_tokenManager = tokenManager;
		}

		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
		{
			var user = await _userManager.FindByNameAsync(registerUserDto.UserName);
			if (user != null)
				return BadRequest("User already exists");

			var newUser = new DemoRestUser
			{
				Email = registerUserDto.Email,
				UserName = registerUserDto.UserName
			};
			var createUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);
			if (!createUserResult.Succeeded)
				return BadRequest("Could not create user.");

			await _userManager.AddToRoleAsync(newUser, DemoRestUserRole.User);
			return CreatedAtAction(nameof(Register), _mapper.Map<UserDto>(newUser));
		}

		[HttpPost]
		[Route("login")]
		public async Task<ActionResult> Login(LoginDto loginDto)
		{
			var user = await _userManager.FindByNameAsync(loginDto.UserName);
			if (user == null)
				return BadRequest("Username or password invalid");

			var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
			if (!isPasswordValid)
				return BadRequest("Username or password invalid");

			var accessToken = await _tokenManager.CreateAcceessTokenAsync(user);

			return Ok(new SuccessfulLoginResponseDto(accessToken));
		}
	}
}
