using Domain.Entities;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Test
{
	class TestRepository<T> : IRepository<T>
		where T : IEntity
	{
		private List<T> _entities = new List<T>();

		public TestRepository() : this (new List<T>())
		{
		}

		public TestRepository(IEnumerable<T> entities)
		{
			_entities = entities.ToList();
		}

		public long Count => _entities.Count;

		public IEnumerable<T> All(Func<T, bool> predicate)
		{
			return _entities.Where(predicate);
		}

		public IEnumerable<T> All()
		{
			return _entities;
		}

		public Task<IEnumerable<T>> AllAsync(Func<T, bool> predicate)
		{
			return Task.FromResult(All(predicate));
		}

		public Task<IEnumerable<T>> AllAsync()
		{
			return Task.FromResult(All());
		}

		public bool Any(Func<T, bool> predicate)
		{
			return _entities.Any(predicate);
		}

		public Task<bool> AnyAsync(Func<T, bool> predicate)
		{
			return Task.FromResult(Any(predicate));
		}

		public T Create(T entity)
		{
			entity.Id = _entities.Count > 0 ? _entities.Max(e => e.Id) + 1 : 0;
			_entities.Add(entity);
			return entity;
		}

		public Task<T> CreateAsync(T entity)
		{
			return Task.FromResult(Create(entity));
		}

		public IEnumerable<T> CreateMany(IEnumerable<T> enities)
		{
			_entities.AddRange(enities);
			return enities;
		}

		public Task<IEnumerable<T>> CreateManyAsync(IEnumerable<T> enities)
		{
			return Task.FromResult(CreateMany(enities));
		}

		public T Delete(T entity)
		{
			if (_entities.Contains(entity))
			{
				_entities.Remove(entity);
			}
			return entity;
		}

		public Task<T> DeleteAsync(T entity)
		{
			return Task.FromResult(Delete(entity));
		}

		public IEnumerable<T> DeleteByIds(params long[] ids)
		{
			var deleted = _entities.Where(e => ids.Contains(e.Id));
			_entities = _entities.Where(e => !ids.Contains(e.Id)).ToList();
			return deleted;
		}

		public Task<IEnumerable<T>> DeleteByIdsAsync(params long[] ids)
		{
			return Task.FromResult(DeleteByIds(ids));
		}

		public T Find(long id)
		{
			return _entities.First(e => e.Id == id);
		}

		public Task<T> FindAsync(long id)
		{
			return Task.FromResult(Find(id));
		}

		public T First(Func<T, bool> predicate)
		{
			return _entities.First(predicate);
		}

		public Task<T> FirstAsync(Func<T, bool> predicate)
		{
			return Task.FromResult(First(predicate));
		}

		public T FirstOrDefault(Func<T, bool> predicate)
		{
			return _entities.FirstOrDefault(predicate);
		}

		public Task<T> FirstOrDefaultAsync(Func<T, bool> predicate)
		{
			return Task.FromResult(FirstOrDefault(predicate));
		}

		public T Update(T entity)
		{
			return entity;
		}

		public Task<T> UpdateAsync(T entity)
		{
			return Task.FromResult(entity);
		}
	}
}
