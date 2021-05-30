using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Implementations;
using UseCases.Implementations.Services;
using Xunit;

namespace UseCases.Test
{
	public class ConversationUserServiceTest
	{
		private readonly Mapper _mapper;
		public ConversationUserServiceTest()
		{
			_mapper = MapperHelpers.GetConfiguredMapper();
		}

		//TODO: wtf with null ref in mapper
		//[Fact]
		public async Task ShouldCreateConversationUser()
		{
			var conversationUserRepository = new TestRepository<ConversationUser>();
			var conversationUserService = new ConversationUserService(conversationUserRepository, _mapper);

			var conversationId = 1;
			var userId = 1;

			await conversationUserService.AddUserAsync(conversationId, userId);

			Assert.True(conversationUserRepository.Any((cu) => cu.ConversationId == conversationId && cu.UserId == userId));
		}

		//TODO: wtf with null ref in mapper
		//[Fact]
		public async Task ShouldDeleteConversationUser()
		{
			var conversationId = 1;
			var userId = 1;
			var conversationUserRepository = new TestRepository<ConversationUser>();
			var conversationUserService = new ConversationUserService(conversationUserRepository, _mapper);

			await conversationUserRepository.CreateAsync(new ConversationUser() { UserId = 1, ConversationId = 1 });
			await conversationUserService.DeleteUserAsync(conversationId, userId);

			Assert.Equal(0, conversationUserRepository.Count);
		}
	}
}
