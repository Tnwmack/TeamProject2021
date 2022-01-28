using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorpSiteAPI.Database.Models;
using NHibernate;

namespace CorpSiteAPI.Database.Context
{
	public class NHibernateMapperSession : IMapperSession
	{
		private readonly ISession _session;
		private ITransaction _transaction;

		public NHibernateMapperSession(ISession session)
		{
			_session = session;
		}

		public IQueryable<Employee> Employees => _session.Query<Employee>();

		public IQueryable<Product> Products => _session.Query<Product>();

		public void BeginTransaction()
		{
			_transaction = _session.BeginTransaction();
		}

		public async Task Commit()
		{
			await _transaction.CommitAsync();
		}

		public async Task Rollback()
		{
			await _transaction.RollbackAsync();
		}

		public void CloseTransaction()
		{
			if (_transaction != null)
			{
				_transaction.Dispose();
				_transaction = null;
			}
		}

		// ##################  EMPLOYEE Entity Methods #############################

		public async Task<int> Save(Employee entity)
		{
			return (int)await _session.SaveAsync(entity);
		}

		public async Task Save(Employee entity, int id)
		{
			await _session.SaveAsync(entity, id);
		}

		public async Task Update(Employee entity)
		{
			await _session.UpdateAsync(entity);
		}

		public async Task Delete(Employee entity)
		{
			await _session.DeleteAsync(entity);
		}

		// ##################  PRODUCT Entity Methods #############################

		public async Task<int> Save(Product entity)
		{
			return (int)await _session.SaveAsync(entity);
		}

		public async Task Save(Product entity, int id)
		{
			await _session.SaveAsync(entity, id);
		}

		public async Task Update(Product entity)
		{
			await _session.UpdateAsync(entity);
		}

		public async Task Delete(Product entity)
		{
			await _session.DeleteAsync(entity);
		}
	}
}
