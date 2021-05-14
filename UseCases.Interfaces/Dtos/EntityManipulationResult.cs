using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Interfaces.Dtos
{
	/// <summary>
	/// Some base shit for responses
	/// </summary>
	/// <typeparam name="T">Entity type, for example Message</typeparam>
	/// <typeparam name="R">FailCause type (enum) is type to determine why tf somethings goes wrong</typeparam>
	public abstract class EntityManipulationResult<T, R> where T : IEntity
	{
		public T Entity { get; set; }
		public bool Success { get; set; }
		public R FailCause { get; set; }
	}
}
