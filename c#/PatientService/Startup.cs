using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Npgsql;
using Microsoft.EntityFrameworkCore;

using PatientService.Data;
using PatientService.Models;

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
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			using (IServiceScope serviceScope = app.ApplicationServices.CreateScope()) {
				ILogger<Startup> logger;
				IServiceProvider serviceProvider;
				PatientServiceDbContext patientServiceDbContext;
				
				serviceProvider = serviceScope.ServiceProvider;
				logger = serviceProvider.GetRequiredService<ILogger<Startup>>();
				patientServiceDbContext = serviceProvider.GetRequiredService<PatientServiceDbContext>();

				try {
					patientServiceDbContext.Database.CanConnect();
				} catch (NpgsqlException e) {
					logger.LogError(e, "An error occured when testing database connection.");
					throw e;
				}

				if (env.IsDevelopment()) {
					app.UseDeveloperExceptionPage();

					logger.LogInformation("Recreating database.");

					try {
						patientServiceDbContext.Database.EnsureDeleted();
						patientServiceDbContext.Database.EnsureCreated();
					} catch (NpgsqlException e) {
						logger.LogError(e, "An error occured when testing database connection.");
						throw e;
					}

					logger.LogInformation("Database recreated.");

					if (bool.Parse(Configuration["SeedDatabase"])) {
						logger.LogInformation("Seeding database.");

						try {
							SeedData.Initialize(serviceProvider);
						} catch (NpgsqlException e) {
							logger.LogError(e, "An error occured while seeding database.");
							throw e;
						} catch (KeyNotFoundException e) {
							logger.LogError(e, "An error occured while parsing json: " + e.Message); 
							throw e;
						} catch (ArgumentNullException e) {
							logger.LogError(e, "An error occured while parsing json: " + e.Message); 
							throw e;
						} catch (InvalidOperationException e) {
							logger.LogError(e, "An error occured while parsing json: " + e.Message); 
							throw e;
						}

						logger.LogInformation("Database successfully seeded.");
					}else {
						logger.LogInformation("Skipping seeding database.");
					}
				} else {
					app.UseExceptionHandler("/Error");
				}
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
