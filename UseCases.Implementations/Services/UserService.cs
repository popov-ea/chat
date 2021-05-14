using DataAccess.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Implementation.Services
{
	public class UserService
	{
		private readonly IRepository<User> _userRepository;
		public UserService(IRepository<User> userRepository)
		{
			_userRepository = userRepository;
		}
	}
}
