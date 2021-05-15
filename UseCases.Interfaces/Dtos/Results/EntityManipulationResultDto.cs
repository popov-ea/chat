namespace UseCases.Interfaces.Dtos
{
	/// <summary>
	/// Some base shit for responses
	/// </summary>
	/// <typeparam name="T">Entity type, for example Message</typeparam>
	/// <typeparam name="R">FailCause type (enum) is type to determine why tf somethings goes wrong</typeparam>
	public abstract class EntityManipulationResultDto<T, R> where T : IEntityDto
	{
		public T Entity { get; set; }
		public bool Success { get; set; }
		public R FailCause { get; set; }
	}
}
