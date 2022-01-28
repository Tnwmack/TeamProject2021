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
	public class ProductMap : ClassMapping<Product>
	{
		public ProductMap()
		{
			Id(x => x.Id, x =>
			{
				x.Type(NHibernateUtil.Int32);
				x.Column("Id");
				x.Generator(Generators.Increment);
			});

			Property(b => b.Name, x =>
			{
				x.Length(100);
				x.Type(NHibernateUtil.StringClob);
				x.NotNullable(true);
			});

			Property(b => b.Quantity, x =>
			{
				x.Length(100);
				x.Type(NHibernateUtil.Int32);
				x.NotNullable(true);
			});

			Property(b => b.Type, x =>
			{
				x.Type(NHibernateUtil.StringClob);
				x.NotNullable(true);
			});

			Property(b => b.Active, x =>
			{
				x.Type(NHibernateUtil.Boolean);
				x.NotNullable(true);
			});

			Property(b => b.ImageLink, x =>
			{
				x.Type(NHibernateUtil.StringClob);
			});
			Property(b => b.Description, x =>
			{
				x.Type(NHibernateUtil.StringClob);
			});

			Table("Products");
		}

	}
}
