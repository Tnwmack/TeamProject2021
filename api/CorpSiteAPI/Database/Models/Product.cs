using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpSiteAPI.Database.Models
{
	public class Product
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual int Quantity { get; set; }
		public virtual string Type { get; set; }
		public virtual bool Active { get; set; }
		public virtual string ImageLink { get; set; }
		public virtual string Description { get; set; }
	}
}
