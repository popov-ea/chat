using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;

namespace DataAccess.EFCore
{
	public class EFCoreRepository<T> : IRepository<T>
		where T : class, IEntity
	{

		public long Count {
			get
			{
				using var db = GetChatDb();
				return db.Set<T>().Count();
			}
		}

		public IEnumerable<T> All(Func<T, bool> predicate)
		{
			using var db = GetChatDb();
			return db.Set<T>().Where(predicate);
		}

		public IEnumerable<T> All()
		{
			using var db = GetChatDb();
			return db.Set<T>().ToList();
		}

		public async Task<IEnumerable<T>> AllAsync(Func<T, bool> predicate)
		{
			using var db = GetChatDb();
			return await db.Set<T>().Where((e) => predicate(e)).ToListAsync();
		}

		public async Task<IEnumerable<T>> AllAsync()
		{
			using var db = GetChatDb();
			return await db.Set<T>().ToListAsync();
		}

		public bool Any(Func<T, bool> predicate)
		{
			using var db = GetChatDb();
			return db.Set<T>().Any(predicate);
		}

		public async Task<bool> AnyAsync(Func<T, bool> predicate)
		{
			using var db = GetChatDb();
			return await db.Set<T>().AnyAsync((e) => predicate(e));
		}

		public T Create(T entity)
		{
			using var db = GetChatDb();
			var set = db.Set<T>();
			var entry = set.Add(entity);
			db.SaveChanges();
			return entry.Entity;
		}

		public async Task<T> CreateAsync(T entity)
		{
			using var db = GetChatDb();
			var set = db.Set<T>();
			var entry = await set.AddAsync(entity);
			await db.SaveChangesAsync();
			return entry.Entity;
		}

		public IEnumerable<T> CreateMany(IEnumerable<T> enities)
		{
			using var db = GetChatDb();
			var set = db.Set<T>();
			set.AddRange(enities);
			db.SaveChanges();

			var entityIds = enities.Select((e) => e.Id);
			return set.Where((e) => entityIds.Contains(e.Id)).ToList();
		}

		public async Task<IEnumerable<T>> CreateManyAsync(IEnumerable<T> enities)
		{
			using var db = GetChatDb();
			var set = db.Set<T>();
			await set.AddRangeAsync(enities);
			await db.SaveChangesAsync();

			var entityIds = enities.Select((e) => e.Id);
			return await set.Where((e) => entityIds.Contains(e.Id)).ToListAsync();
		}

		public T Delete(T entity)
		{
			using var db = GetChatDb();
			var toDelete = db.Find<T>(entity.Id);
			db.Remove(toDelete);
			db.SaveChanges();
			return toDelete;
		}

		public async Task<T> DeleteAsync(T entity)
		{
			using var db = GetChatDb();
			var toDelete = await db.FindAsync<T>(entity.Id);
			db.Remove(toDelete);
			await db.SaveChangesAsync();
			return toDelete;
		}

		public IEnumerable<T> DeleteByIds(params long[] ids)
		{
			using var db = GetChatDb();
			var set = db.Set<T>();
			var toDelete = set.Where((e) => ids.Contains(e.Id));
			set.RemoveRange(toDelete);
			db.SaveChanges();
			return toDelete;
		}

		public async Task<IEnumerable<T>> DeleteByIdsAsync(params long[] ids)
		{
			using var db = GetChatDb();
			var set = db.Set<T>();
			var toDelete = await set.Where((e) => ids.Contains(e.Id)).ToListAsync();
			set.RemoveRange(toDelete);
			await db.SaveChangesAsync();
			return toDelete;
		}

		public T Find(long id)
		{
			using var db = GetChatDb();
			return db.Find<T>(id);
		}

		public async Task<T> FindAsync(long id)
		{
			using var db = GetChatDb();
			return await db.FindAsync<T>(id);
		}

		public T First(Func<T, bool> predicate)
		{
			using var db = GetChatDb();
			return db.Set<T>().First(predicate);
		}

		public async Task<T> FirstAsync(Func<T, bool> predicate)
		{
			using var db = GetChatDb();
			return await db.Set<T>().FirstAsync((e) => predicate(e));
		}

		public T FirstOrDefault(Func<T, bool> predicate)
		{
			using var db = GetChatDb();
			return db.Set<T>().FirstOrDefault((e) => predicate(e));
		}

		public async Task<T> FirstOrDefaultAsync(Func<T, bool> predicate)
		{
			using var db = GetChatDb();
			return await db.Set<T>().FirstOrDefaultAsync((e) => predicate(e));
		}

		public T Update(T entity)
		{
			using var db = GetChatDb();
			db.Attach<T>(entity);
			db.Entry<T>(entity).State = EntityState.Modified;
			db.SaveChanges();
			return entity;
		}

		public async Task<T> UpdateAsync(T entity)
		{
			using var db = GetChatDb();
			db.Attach(entity);
			db.Entry(entity).State = EntityState.Modified;
			await db.SaveChangesAsync();
			return entity;
		}

		private ChatDb GetChatDb() => new ChatDb();
	}
}
