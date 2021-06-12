using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Implementations.Services;
using UseCases.Implementations;
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

		[Fact]
		public async Task ShouldCreateConversationUser()
		{
			var firedEventsCount = 0;
			var conversationUserRepository = new TestRepository<ConversationUser>();
			var conversationUserService = new ConversationUserService(conversationUserRepository, _mapper);

			var conversationId = 1;
			var userId = 1;

			conversationUserService.OnUserAdded += (cu) => firedEventsCount++;

			await conversationUserService.AddUserAsync(conversationId, userId);

			Assert.Contains(conversationUserRepository.All(), (cu) => cu.ConversationId == conversationId && cu.UserId == userId);
			Assert.Equal(1, firedEventsCount);
		}

		[Fact]
		public async Task ShouldDeleteConversationUser()
		{
			var firedEventsCount = 0;
			var conversationId = 1;
			var userId = 1;
			var conversationUserRepository = new TestRepository<ConversationUser>();
			var conversationUserService = new ConversationUserService(conversationUserRepository, _mapper);

			conversationUserService.OnUserDeleted += (cu) => firedEventsCount++;

			await conversationUserRepository.CreateAsync(new ConversationUser() { UserId = 1, ConversationId = 1 });
			await conversationUserService.DeleteUserAsync(conversationId, userId);

			Assert.Empty(conversationUserRepository.All());
			Assert.Equal(1, firedEventsCount);
		}
	}
}
