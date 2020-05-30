using System;
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

			services.AddScoped<IPatientServiceDbHandler, PatientServiceDbHandler>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IPatientServiceDbHandler patientServiceDbHandler)
		{
			using (IServiceScope serviceScope = app.ApplicationServices.CreateScope()) {
				ILogger<Startup> logger;
				IServiceProvider serviceProvider;
				
				serviceProvider = serviceScope.ServiceProvider;
				logger = serviceProvider.GetRequiredService<ILogger<Startup>>();

				try {
					patientServiceDbHandler.CanConnect();
				} catch (NpgsqlException e) {
					logger.LogError(e, "An error occured when testing database connection.");
					throw e;
				}

				if (env.IsDevelopment()) {
					app.UseDeveloperExceptionPage();

					logger.LogInformation("Recreating database.");

					try {
						patientServiceDbHandler.EnsureDeleted();
						patientServiceDbHandler.EnsureCreated();
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
