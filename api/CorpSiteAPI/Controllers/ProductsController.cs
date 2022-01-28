using System;
using System.IO;
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
using Microsoft.AspNetCore.Http;
using CorpSiteAPI.Controllers.Converters;
using Microsoft.Extensions.Hosting;

namespace CorpSiteAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnv;
        private readonly Database.Context.IMapperSession _session;

        public ProductsController(Microsoft.AspNetCore.Hosting.IWebHostEnvironment env, Database.Context.IMapperSession session)
        {
            _hostingEnv = env;
            _session = session;
        }

        private IQueryable<Database.Models.Product> BuildQueryFromFilter(string[] filter)
        {
            IQueryable<Database.Models.Product> query = (from p in _session.Products
                                         select p);

            if (filter == null || filter.Length == 0)
                return query;

            foreach(string filterParam in filter)
            {
                int idValue = 0;

                if (int.TryParse(filterParam, out idValue))
                {
                    query = query.Where(p => (p.Id == idValue || p.Name.Contains(filterParam)));
                }
                else if(filterParam.ToLower() == "hideinactive")
                {
                    query = query.Where(p => (p.Active == true));
                }
                else
                {
                    query = query.Where(p => (p.Name.ToLower().Contains(filterParam.ToLower())));
                }
            }

            return query;
        }

        private IQueryable<Database.Models.Product> AddOrderToQuery(string order, IQueryable<Database.Models.Product> queryIn)
        {
            IQueryable<Database.Models.Product> defaultOrder = queryIn.OrderBy(p => p.Id);

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
                        queryIn = queryIn.OrderBy(p => p.Id);
                    else
                        queryIn = queryIn.OrderByDescending(p => p.Id);
                    break;

                case "name":
                    if (ascending)
                        queryIn = queryIn.OrderBy(p => p.Name);
                    else
                        queryIn = queryIn.OrderByDescending(p => p.Name);
                    break;

                case "quantity":
                    if (ascending)
                        queryIn = queryIn.OrderBy(p => p.Quantity).ThenBy(p => p.Name);
                    else
                        queryIn = queryIn.OrderByDescending(p => p.Quantity).ThenBy(p => p.Name);
                    break;

                case "type":
                    if (ascending)
                        queryIn = queryIn.OrderBy(p => p.Type).ThenBy(p => p.Name);
                    else
                        queryIn = queryIn.OrderByDescending(p => p.Type).ThenBy(p => p.Name);
                    break;

                default:
                    return defaultOrder;
            }

            return queryIn;
        }

        /// <summary>
        /// List all products
        /// </summary>
        /// <remarks>Returns all of the products with filtering and paging.</remarks>
        /// <param name="filter">Filtering selection string.</param>
        /// <param name="order">List order, formatted like 'column_asc/dec', eg, name_asc</param>
        /// <param name="offset">The offset of the first item that should be returned.</param>
        /// <param name="size">The maximum number of items that can be returned.</param>
        /// <response code="200">Product page view.</response>
        /// <response code="400">The filtering or paging request was bad.</response>
        [HttpGet]
        [Route("/api/products")]
        [ValidateModelState]
        [SwaggerOperation("ProductsGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(ProductPage), description: "Product page view.")]
        public virtual IActionResult ProductsGet([FromQuery] string[] filter, [FromQuery] string order, [FromQuery] int? offset, [FromQuery] int? size)
        {
            IQueryable<Database.Models.Product> query;

            int queryStart = offset.GetValueOrDefault();
            int querySize = size.GetValueOrDefault(10);

            query = BuildQueryFromFilter(filter);
            query = AddOrderToQuery(order, query);

            int totalSize = query.Count();

            query = query
                .Skip(queryStart)
                .Take(querySize);

            var queryList = query.ToList();
            var resultList = queryList.ConvertAll(p => p.ToControllerProduct());

            var result = new ProductPage()
            {
                Products = resultList,
                Total = totalSize,
                Offset = queryStart
            };

            return new ObjectResult(result);
        }

        /// <summary>
        /// Get a product
        /// </summary>
        /// <param name="id">The id number of the object.</param>
        /// <response code="200">Product data</response>
        /// <response code="404">The product id was not found.</response>
        [HttpGet]
        [Route("/api/products/{id}")]
        [ValidateModelState]
        [SwaggerOperation("ProductsIdGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(Models.Product), description: "Product data")]
        public virtual IActionResult ProductsIdGet([FromRoute][Required] int? id)
        {
            var query = (from p in _session.Products
                          where p.Id == id
                          select p);

            if (query.Count() != 1)
            {
                return NotFound();
            }

            var queryList = query.ToList();

            return new ObjectResult(queryList[0].ToControllerProduct());
        }

        /// <summary>
        /// Add or update a product
        /// </summary>
        /// <remarks>Adds a new product to the database with the given id, or if the product id already exists it will be modified.</remarks>
        /// <param name="id">The id number of the object.</param>
        /// <param name="body"></param>
        /// <response code="200">Product data</response>
        /// <response code="400">The product data given was invalid.</response>
        [HttpPut]
        [Route("/api/products/{id}")]
        [ValidateModelState]
        [SwaggerOperation("ProductsIdPut")]
        [SwaggerResponse(statusCode: 200, type: typeof(Models.Product), description: "Product data")]
        public async virtual Task<IActionResult> ProductsIdPut([FromRoute][Required] int? id, [FromBody] NewProduct body)
        {
            bool update = false;

            var query = (from p in _session.Products
                         where p.Id == id.Value
                         select p);

            if (query.Count() == 1)
                update = true;

            Database.Models.Product newProduct = body.ToDatabaseProduct(id.Value);

            try
            {
                _session.BeginTransaction();

                if (update)
                    await _session.Update(newProduct);
                else
                    await _session.Save(newProduct, id.Value);
                
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

            return new ObjectResult(newProduct.ToControllerProduct());
        }

        /// <summary>
        /// Add a new product
        /// </summary>
        /// <remarks>Adds a new product to the database, a new product id will be automatically created.</remarks>
        /// <param name="body"></param>
        /// <response code="200">Product data</response>
        /// <response code="400">The product data given was invalid.</response>
        [HttpPost]
        [Route("/api/products")]
        [ValidateModelState]
        [SwaggerOperation("ProductsPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(Models.Product), description: "Product data")]
        public async virtual Task<IActionResult> ProductsPost([FromBody] NewProduct body)
        {
            Database.Models.Product newProduct = body.ToDatabaseProduct(0);

            try
            {
                _session.BeginTransaction();
                await _session.Save(newProduct);
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

            return new ObjectResult(newProduct.ToControllerProduct());
        }

        /// <summary>
        /// Add a new product image to the static image storage
        /// </summary>
        /// <remarks>Adds a new product image to the temp folder, returns an image link.</remarks>
        /// <param name="body"></param>
        /// <response code="200">Product Image Link</response>
        /// <response code="400">The product image file given was invalid.</response>
        [HttpPost]
        [Route("/api/products/image")]
        [ValidateModelState]
        [SwaggerOperation("ProductsImagePost")]
        [SwaggerResponse(statusCode: 200, type: typeof(String), description: "Image URL")]
        public async virtual Task<IActionResult> ProductsImagePost([FromForm] NewProductImage body)
        {
            string ImageLink = $"temp/{Path.GetRandomFileName()}.png";
            string basePath;

            if(_hostingEnv.IsDevelopment())
            {
                basePath = "../../web/public/products/";
            }
            else
            {
                basePath = "web/products/";
            }

            //TODO: for production, err check and virus scanning, etc!!!
            //      and also make path configurable.
            string filePath = Path.Combine(basePath, ImageLink);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await body.ProductImage.CopyToAsync(fileStream);
            }

            return new ObjectResult(ImageLink);
        }
    }
}
