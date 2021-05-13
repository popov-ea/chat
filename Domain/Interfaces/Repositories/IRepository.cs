using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
	public interface IRepository<T> where T : IEntity
	{
		public T FirstOrDefault(Func<T, bool> predicate);
		public Task<T> FirstOrDefaultAsync(Func<T, bool> predicate);

		public T First(Func<T, bool> predicate);
		public Task<T> FirstAsync(Func<T, bool> predicate);

		public IEnumerable<T> All(Func<T, bool> predicate);
		public Task<IEnumerable<T>> AllAsync(Func<T, bool> predicate);

		public IEnumerable<T> All();
		public Task<IEnumerable<T>> AllAsync();

		public bool Any(Func<T, bool> predicate);
		public Task<bool> AnyAsync(Func<T, bool> predicate);

		public T Find(long id);
		public Task<T> FindAsync(long id);

		public T Create(T entity);
		public Task<T> CreateAsync(T entity);

		public IEnumerable<T> CreateMany(IEnumerable<T> enities);
		public Task<IEnumerable<T>> CreateManyAsync(IEnumerable<T> enities);

		public IEnumerable<T> DeleteByIds(params long[] ids);
		public Task<IEnumerable<T>> DeleteByIdsAsync(params long[] ids);

		public T Update(T entity);
		public Task<T> UpdateAsync(T entity);

		public T Delete(T entity);
		public Task<T> DeleteAsync(T entity);

		public long Count { get; }
	}
}
