using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpSiteAPI.Database.Models
{
	public class Employee
	{
		public virtual int Id { get; set; }
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string Title { get; set; }
		public virtual string Email { get; set; }
		public virtual string Bio { get; set; }
		public virtual DateTime HireDate { get; set; }
		public virtual bool Active { get; set; }

	}
}
