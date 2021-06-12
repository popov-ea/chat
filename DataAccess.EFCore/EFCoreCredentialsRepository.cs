using Auth.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore
{
	public class EFCoreCredentialsRepository : ICredentialsRepository
	{
		private readonly DbConfig _dbConfig;

		public EFCoreCredentialsRepository(DbConfig dbConfig)
		{
			_dbConfig = dbConfig;
		}

		public async Task<bool> CheckLoginExistsAsync(string login)
		{
			using var db = GetChatDb();
			return await db.UserCredentials.AnyAsync((uc) => uc.Login == login);
		}

		public async Task<UserCredentialsDto> CreateCredentialsAsync(string login, string password)
		{
			using var db = GetChatDb();
			var entity = (await db.UserCredentials.AddAsync(new UserCredentials
			{
				Login = login,
				Password = password
			})).Entity;
			return new UserCredentialsDto
			{
				Id = entity.Id,
				UserId = entity.UserId,
				Login = entity.Login,
				//TODO: А нужно ли это
				Password = entity.Password
			};
		}

		public async Task<UserCredentialsDto> GetCredentialsAsync(string login)
		{
			using var db = GetChatDb();
			var entity = await db.UserCredentials.FirstOrDefaultAsync((uc) => uc.Login == login);
			return new UserCredentialsDto
			{
				Id = entity.Id,
				UserId = entity.UserId,
				Login = entity.Login,
				//TODO: А нужно ли это
				Password = entity.Password
			};
		}

		private ChatDb GetChatDb() => new ChatDb(_dbConfig);
	}
}
