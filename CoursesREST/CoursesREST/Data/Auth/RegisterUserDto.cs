using System.ComponentModel.DataAnnotations;

namespace CoursesREST.Data.Auth
{
	public record RegisterUserDto([Required] string UserName, [EmailAddress][Required] string Email, [Required] string Password);
}
