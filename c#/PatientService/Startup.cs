using System;
using System.Configuration;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;
using Microsoft.EntityFrameworkCore;

using PatientService.Data;
using PatientService.Models;
using PatientService.Services;

namespace PatientService
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHttpClient();
			
			services.AddControllers();

			services.AddDbContext<PatientServiceDbContext>(options =>
				options.UseNpgsql(Configuration.GetConnectionString("PatientServiceDbContext")));

			if (string.IsNullOrEmpty(Configuration["ORM"]) || Configuration["ORM"] == "EfCore") {
				services.AddScoped<IPatientServiceDbService, PatientServiceEfCoreDbService>();
			} else {
				services.AddScoped<IPatientServiceDbService, PatientServiceDapperDbService>();
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			ILogger<Startup> logger;

			logger = app.ApplicationServices.CreateScope().ServiceProvider
				.GetRequiredService<ILogger<Startup>>();


			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/error");
			}
			
			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
