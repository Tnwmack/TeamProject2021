using CorpSiteAPI.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpSiteAPI.Database.Context
{
	public interface IMapperSession
	{
		void BeginTransaction();
		Task Commit();
		Task Rollback();
		void CloseTransaction();

		// ##################  EMPLOYEE Mapper Interface #############################

		Task<int> Save(Employee entity);
		Task Save(Employee entity, int id);
		Task Delete(Employee entity);
		Task Update(Employee entity);

		IQueryable<Employee> Employees { get; }

		// ##################  PRODUCT Mapper Interface #############################

		Task<int> Save(Product entity);
		Task Save(Product entity, int id);
		Task Update(Product entity);
		Task Delete(Product entity);

		IQueryable<Product> Products { get; }
	}
}
