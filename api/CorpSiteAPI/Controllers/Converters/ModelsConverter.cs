using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorpSiteAPI.Controllers.Converters
{
	public static class ModelsConverter
	{
		public static Models.Product ToControllerProduct(this Database.Models.Product product)
		{
			if (string.IsNullOrWhiteSpace(product.Name))
				throw new ArgumentNullException(nameof(product), nameof(Database.Models.Product.Name));

			if (string.IsNullOrWhiteSpace(product.Type))
				throw new ArgumentNullException(nameof(product), nameof(Database.Models.Product.Type));

			var result = new Models.Product
			{
				Id = product.Id,
				Name = product.Name,
				Quantity = product.Quantity,
				Type = product.Type,
				ImageLink = product.ImageLink,
				Description = product.Description,
				Active = product.Active
			};

			return result;
		}

		public static Models.Employee ToControllerEmployee(this Database.Models.Employee employee)
		{
			if (string.IsNullOrWhiteSpace(employee.FirstName))
				throw new ArgumentNullException(nameof(employee), nameof(Database.Models.Employee.FirstName));

			if (string.IsNullOrWhiteSpace(employee.LastName))
				throw new ArgumentNullException(nameof(employee), nameof(Database.Models.Employee.LastName));

			if (string.IsNullOrWhiteSpace(employee.Title))
				throw new ArgumentNullException(nameof(employee), nameof(Database.Models.Employee.Title));

			var result = new Models.Employee
			{
				Id = employee.Id,
				FirstName = employee.FirstName,
				LastName = employee.LastName,
				Title = employee.Title,
				Email = employee.Email,
				Bio = employee.Bio,
				HireDate = employee.HireDate,
				Active = employee.Active
			};

			return result;
		}

		public static Database.Models.Product ToDatabaseProduct(this Models.NewProduct product, int id)
		{
			var result = new Database.Models.Product
			{
				Id = id,
				Name = product.Name,
				Quantity = product.Quantity.GetValueOrDefault(),
				Type = product.Type,
				ImageLink = product.ImageLink,
				Description = product.Description,
				Active = product.Active.GetValueOrDefault(true)
			};

			return result;
		}

		public static Database.Models.Employee ToDatabaseEmployee(this Models.NewEmployee employee, int id)
		{
			var result = new Database.Models.Employee
			{
				Id = id,
				FirstName = employee.FirstName,
				LastName = employee.LastName,
				Title = employee.Title,
				Email = employee.Email,
				Bio = employee.Bio,
				HireDate = employee.HireDate.GetValueOrDefault(),
				Active = employee.Active.GetValueOrDefault(true)
			};

			return result;
		}
	}
}
