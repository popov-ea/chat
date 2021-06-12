using System.Threading.Tasks;

namespace Auth.Interfaces
{
	public interface ICredentialsRepository
	{
		public Task<UserCredentialsDto> GetCredentialsAsync(string login);
		public Task<bool> CheckLoginExistsAsync(string login);
		public Task<UserCredentialsDto> CreateCredentialsAsync(string login, string password);
	}
}