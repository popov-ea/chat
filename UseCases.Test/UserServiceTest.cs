using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Implementations;
using UseCases.Implementations.Services;
using UseCases.Interfaces.Dtos;
using Xunit;

namespace UseCases.Test
{
	public class UserServiceTest
	{
		[Fact]
		public async Task ShouldCreateUser()
		{
			var dto = new UserDto
			{
				UserName = "test"
			};

			var mapper = MapperHelpers.GetConfiguredMapper();
			var repository = new TestRepository<User>();
			var userService = new UserService(repository, mapper);

			await userService.CreateUserAsync(dto);

			Assert.Contains(await repository.AllAsync(), (u) => u.Username == dto.UserName);
		}
	}
}
