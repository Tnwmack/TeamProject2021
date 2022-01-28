using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using CorpSiteAPI.Controllers.Attributes;
using CorpSiteAPI.Controllers.Models;
using System.Linq;
using System.Threading.Tasks;
using CorpSiteAPI.Controllers.Converters;

namespace CorpSiteAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private readonly Database.Context.IMapperSession _session;

        public EmployeesController(Database.Context.IMapperSession session)
        {
            _session = session;
        }

        private IQueryable<Database.Models.Employee> BuildQueryFromFilter(string[] filter)
        {
            IQueryable<Database.Models.Employee> query = (from e in _session.Employees
                                          select e);

            if (filter == null || filter.Length == 0)
                return query;

            foreach (string filterParam in filter)
            {
                int idValue = 0;

                if (int.TryParse(filterParam, out idValue))
                {
                    query = query.Where(e => (e.Id == idValue || 
                        e.FirstName.Contains(filterParam) || 
                        e.LastName.Contains(filterParam) || 
                        e.Title.Contains(filterParam)));
                }
                else if (filterParam.ToLower() == "hideinactive")
                {
                    query = query.Where(e => (e.Active == true));
                }
                else
                {
                    query = query.Where(e => (e.FirstName.ToLower().Contains(filterParam.ToLower()) || 
                        e.LastName.ToLower().Contains(filterParam.ToLower()) || 
                        e.Title.ToLower().Contains(filterParam.ToLower())));
                }
            }

            return query;
        }

        private IQueryable<Database.Models.Employee> AddOrderToQuery(string order, IQueryable<Database.Models.Employee> queryIn)
		{
            IQueryable<Database.Models.Employee> defaultOrder = queryIn.OrderBy(e => e.Id);

            if (string.IsNullOrWhiteSpace(order))
                return defaultOrder;

            bool ascending = true;
            string[] orderSplit = order.Split('_');

            if (orderSplit.Length != 2)
                return defaultOrder;

            if (orderSplit[1] == "dec")
                ascending = false;
            else if (orderSplit[1] != "asc")
                return defaultOrder;

            switch (orderSplit[0])
            {
                case "id":
                    if (ascending)
                        queryIn = queryIn.OrderBy(e => e.Id);
                    else
                        queryIn = queryIn.OrderByDescending(e => e.Id);
                    break;

                case "firstname":
                    if (ascending)
                        queryIn = queryIn.OrderBy(e => e.FirstName).ThenBy(e => e.LastName);
                    else
                        queryIn = queryIn.OrderByDescending(e => e.FirstName).ThenBy(e => e.LastName);
                    break;

                case "lastname":
                    if (ascending)
                        queryIn = queryIn.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
                    else
                        queryIn = queryIn.OrderByDescending(e => e.LastName).ThenBy(e => e.FirstName);
                    break;

                case "title":
                    if (ascending)
                        queryIn = queryIn.OrderBy(e => e.Title).ThenBy(e => e.Id);
                    else
                        queryIn = queryIn.OrderByDescending(e => e.Title).ThenBy(e => e.Id);
                    break;

                case "hiredate":
                    if (ascending)
                        queryIn = queryIn.OrderBy(e => e.HireDate).ThenBy(e => e.Id);
                    else
                        queryIn = queryIn.OrderByDescending(e => e.HireDate).ThenBy(e => e.Id);
                    break;

                default:
                    return defaultOrder;
            }

            return queryIn;
        }

        /// <summary>
        /// List all employees
        /// </summary>
        /// <remarks>Returns all of the employees with filtering and paging.</remarks>
        /// <param name="filter">Filtering selection string.</param>
        /// <param name="startIndex">The index of the first result that should be returned.</param>
        /// <param name="size">The maximum number of items that can be returned.</param>
        /// <response code="200">Employee page view.</response>
        /// <response code="400">The filtering or paging request was bad.</response>
        [HttpGet]
        [Route("/api/employees")]
        [ValidateModelState]
        [SwaggerOperation("EmployeesGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(EmployeePage), description: "Employee page view.")]
        public virtual IActionResult EmployeesGet([FromQuery] string[] filter, [FromQuery] string order, [FromQuery] int? offset, [FromQuery] int? size)
        {
            IQueryable<Database.Models.Employee> query;

            int queryStart = offset.GetValueOrDefault();
            int querySize = size.GetValueOrDefault(10);

            query = BuildQueryFromFilter(filter);
            query = AddOrderToQuery(order, query);

            int totalSize = query.Count();

            query = query
                .Skip(queryStart)
                .Take(querySize);

            var queryList = query.ToList();
            var resultList = queryList.ConvertAll(e => e.ToControllerEmployee());

            var result = new EmployeePage()
            {
                Employees = resultList,
                Total = totalSize,
                Offset = queryStart
            };

            return new ObjectResult(result);
        }

        /// <summary>
        /// Get an employee
        /// </summary>
        /// <param name="id">The id number of the object.</param>
        /// <response code="200">Employee data</response>
        /// <response code="404">The employee id was not found.</response>
        [HttpGet]
        [Route("/api/employees/{id}")]
        [ValidateModelState]
        [SwaggerOperation("EmployeesIdGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(Models.Employee), description: "Employee data")]
        public virtual IActionResult EmployeesIdGet([FromRoute][Required] int? id)
        {
            var query = (from e in _session.Employees
                         where e.Id == id
                         select e);

            if (query.Count() != 1)
            {
                return NotFound();
            }

            var queryList = query.ToList();

            return new ObjectResult(queryList[0].ToControllerEmployee());
        }

        /// <summary>
        /// Add or update an employee
        /// </summary>
        /// <remarks>Adds a new employee to the database with the given id, or if the employee id already exists it will be modified.</remarks>
        /// <param name="id">The id number of the object.</param>
        /// <param name="body"></param>
        /// <response code="200">Employee data</response>
        /// <response code="400">The employee data given was invalid.</response>
        [HttpPut]
        [Route("/api/employees/{id}")]
        [ValidateModelState]
        [SwaggerOperation("EmployeesIdPut")]
        [SwaggerResponse(statusCode: 200, type: typeof(Models.Employee), description: "Employee data")]
        public async virtual Task<IActionResult> EmployeesIdPut([FromRoute][Required] int? id, [FromBody] NewEmployee body)
        {
            bool update = false;

            var query = (from e in _session.Employees
                         where e.Id == id.Value
                         select e);

            if (query.Count() == 1)
                update = true;

            Database.Models.Employee newEmployee = body.ToDatabaseEmployee(id.Value);

            try
            {
                _session.BeginTransaction();

                if (update)
                    await _session.Update(newEmployee);
                else
                    await _session.Save(newEmployee, id.Value);

                await _session.Commit();
            }

            catch (Exception)
            {
                await _session.Rollback();
                throw;
            }

            finally
            {
                _session.CloseTransaction();
            }

            return new ObjectResult(newEmployee.ToControllerEmployee());
        }

        /// <summary>
        /// Add a new employee
        /// </summary>
        /// <remarks>Adds a new employee to the database, a new employee id will be automatically created.</remarks>
        /// <param name="body"></param>
        /// <response code="200">Employee data</response>
        /// <response code="400">The employee data given was invalid.</response>
        [HttpPost]
        [Route("/api/employees")]
        [ValidateModelState]
        [SwaggerOperation("EmployeesPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(Models.Employee), description: "Employee data")]
        public async virtual Task<IActionResult> EmployeesPost([FromBody] NewEmployee body)
        {
            Database.Models.Employee newEmployee = body.ToDatabaseEmployee(0);

            try
            {
                _session.BeginTransaction();
                await _session.Save(newEmployee);
                await _session.Commit();
            }

            catch (Exception)
            {
                await _session.Rollback();
                throw;
            }

            finally
            {
                _session.CloseTransaction();
            }

            return new ObjectResult(newEmployee.ToControllerEmployee());
        }
    }
}
