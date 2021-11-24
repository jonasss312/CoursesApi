using CoursesREST.Data;
using CoursesREST.Data.Auth;
using CoursesREST.Data.Auth.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesREST
{
	public class Startup
	{
		private IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddIdentity<DemoRestUser, IdentityRole>()
				.AddEntityFrameworkStores<Context>()
				.AddDefaultTokenProviders();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
				options.TokenValidationParameters.ValidAudience = _configuration["JWT:ValidAudience"];
				options.TokenValidationParameters.ValidIssuer = _configuration["JWT:ValidIssuer"];
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(PolicyNames.SameUser, policy => policy.Requirements.Add(new SameUserRequirement()));
			});

			services.AddSingleton<IAuthorizationHandler, SameUserAuthorizationHandler>();
			services.AddDbContext<Context>();
			services.AddAutoMapper(typeof(Startup));
			services.AddControllers();
			services.AddTransient<ICategoriesRepository, CategoriesRepository>();
			services.AddTransient<ICoursesRepository, CoursesRepository>();
			services.AddTransient<ICommentsRepository, CommentsRepository>();
			services.AddTransient<ITokenManager, TokenManager>();
			services.AddTransient<DatabaseSeeder, DatabaseSeeder>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseAuthorization();
			app.UseAuthentication();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
