using AutoMapper;
using CoursesREST.Data.Auth;
using CoursesREST.Data.Auth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesREST.Data
{
	public class MapperConfg : Profile
	{
		public MapperConfg()
		{
			CreateMap<DemoRestUser, UserDto>();
		}
	}
}
