using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate;
using NHibernate.Mapping.ByCode;
using CorpSiteAPI.Database.Models;

namespace CorpSiteAPI.Database.Mappings
{
	public class EmployeeMap : ClassMapping<Employee>
	{
		public EmployeeMap()
		{
			Id(x => x.Id, x =>
			{
				x.Type(NHibernateUtil.Int32);
				x.Column("Id");
				x.Generator(Generators.Increment);
			});

			Property(b => b.FirstName, x =>
			{
				x.Length(100);
				x.Type(NHibernateUtil.StringClob);
				x.NotNullable(true);
			});


			Property(b => b.Title, x =>
			{
				x.Length(100);
				x.Type(NHibernateUtil.StringClob);
				x.NotNullable(true);
			});

			Property(b => b.Email, x =>
			{
				x.Length(100);
				x.Type(NHibernateUtil.StringClob);
				x.NotNullable(false);
			});

			Property(b => b.HireDate, x =>
			{
				x.Type(NHibernateUtil.Date);
				x.NotNullable(true);
			});

			Property(b => b.LastName, x =>
			{
				x.Length(100);
				x.Type(NHibernateUtil.StringClob);
				x.NotNullable(true);
			});

			Property(b => b.Active, x =>
			{
				x.Type(NHibernateUtil.Boolean);
				x.NotNullable(true);
			});

			Property(b => b.Bio, x =>
			{
				x.Type(NHibernateUtil.StringClob);
			});

			Table("Employee");
		}

	}
}
