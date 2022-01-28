using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using CorpSiteAPI.Controllers.Filters;
using System.IO;
using CorpSiteAPI.Database.Context;

namespace CorpSiteAPI
{
	public class Startup
	{
		private readonly IWebHostEnvironment _hostingEnv;

		public Startup(IWebHostEnvironment env, IConfiguration configuration)
		{
			_hostingEnv = env;
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			string connectionString;

			if (_hostingEnv.IsDevelopment())
			{
				connectionString = "Data Source=bin/Debug/net5.0/CorpSite_Database.db;";
			}
			else
			{
				connectionString = "Data Source=CorpSite_Database.db;";
			}

			services.AddNHibernate(connectionString);
			services.AddControllers();

			services.AddCors();

			services
				.AddMvc(options =>
				{
					options.InputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonInputFormatter>();
					options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter>();
				})
				.AddNewtonsoftJson(opts =>
				{
					opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					opts.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
				})
				.AddXmlSerializerFormatters();

			services
				.AddSwaggerGen(c =>
				{
					c.SwaggerDoc("1.0.0", new OpenApiInfo
					{
						Version = "1.0.0",
						Title = "CorpSite API",
						Description = "CorpSite API (ASP.NET Core 3.1)",
						Contact = new OpenApiContact()
						{
							Name = "Swagger Codegen Contributors",
							Url = new Uri("https://github.com/swagger-api/swagger-codegen"),
							Email = ""
						}
					});
					c.CustomSchemaIds(type => type.FullName);
					c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");
					// Sets the basePath property in the Swagger document generated
					c.DocumentFilter<BasePathFilter>("/api");

					// Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
					// Use [ValidateModelState] on Actions to actually validate it in C# as well!
					c.OperationFilter<GeneratePathParamsValidationFilter>();
				});
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/1.0.0/swagger.json", "CorpSiteAPI v1"));
			}

			app.UseRouting();

			app.UseCors(options =>
			{
				options.AllowAnyOrigin();
				options.AllowAnyHeader();
				options.AllowAnyMethod();
			});

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
