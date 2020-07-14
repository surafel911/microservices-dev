using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Microsoft.EntityFrameworkCore;

using PatientService.Data;
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

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHttpClient();
			services.AddControllers();
			services.AddDbContext<PatientDbContext>(options =>
				options.UseNpgsql(Configuration.GetConnectionString("PatientDbContext")));

			switch (ConfigurationBinder.GetValue<int>(Configuration, "PATIENTSERVICE_ORM")) {
			case 2:
				services.AddScoped<IPatientDbService, PatientDapperDbService>();
				services.AddHealthChecks()
					.AddNpgSql(Configuration.GetConnectionString("PatientDbContext"));
				break;
			default:
				services.AddScoped<IPatientDbService, PatientEfCoreDbService>();
				services.AddHealthChecks()
					.AddDbContextCheck<PatientDbContext>();
				break;
			}

			// TODO: Add health check monitoring.
		}

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
				endpoints.MapHealthChecks("/health");
			});
		}
	}
}
