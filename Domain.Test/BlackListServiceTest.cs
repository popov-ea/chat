using Domain.Services;
using System;
using Xunit;
using Domain.Interfaces.Repositories;
using Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Test
{
	public class BlackListServiceTest
	{
		[Fact]
		public async Task ShouldBlockUser()
		{
			var initiator = new User
			{
				Id = 1
			};
			var blocked = new User
			{
				Id = 2
			};
			var blackListRepository = new TestRepository<BlackList>();
			var userRepository = new TestRepository<User>(new List<User>
			{
				initiator,
				blocked
			});
			var blackListService = new BlackListService(blackListRepository);

			await blackListService.BlockUserAsync(initiator, blocked);

			Assert.True(blackListRepository.Any(bl => bl.InitiatorId == initiator.Id && bl.BlockedId == blocked.Id));
		}

		[Fact]
		public async Task ShouldUnblockUser()
		{
			var initiator = new User
			{
				Id = 1
			};
			var blocked = new User
			{
				Id = 2
			};
			var userRepository = new TestRepository<User>(new List<User>
			{
				initiator,
				blocked
			});
			var blackListRepository = new TestRepository<BlackList>(new List<BlackList>()
			{
				new BlackList
				{
					Id = 1,
					Blocked = blocked,
					Initiator = initiator,
					BlockedId = blocked.Id,
					InitiatorId = initiator.Id
				}
			});
			var blackListService = new BlackListService(blackListRepository);

			await blackListService.UnblockUserAsync(initiator, blocked);

			Assert.True(!blackListRepository.Any(bl => bl.InitiatorId == initiator.Id && bl.BlockedId == blocked.Id));
		}
	}
}
