using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PatientService.Data;
using PatientService.Models;
using PatientService.Services;

namespace PatientService
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		private void SetupDapperORM(IServiceCollection services)
		{
			IDictionary<DbConnectionName, string> connectionDictionary = new Dictionary<DbConnectionName, string>
			{
				{DbConnectionName.DefaultDbName, Configuration.GetConnectionString("DefaultDbContext")},
				{DbConnectionName.PatientDbName, Configuration.GetConnectionString("PatientDbContext")}
			};
			
			services.AddSingleton(connectionDictionary);
			services.AddTransient<IDbConnectionFactory, DapperDbConnectionFactory>();

			services.AddTransient<IDefaultDbService, DefaultDbService>();
			services.AddTransient<IPatientDbCommandService, PatientDbCommandService>();
			
			services.AddScoped<IDefaultDbService, DefaultDbService>();
			services.AddScoped<IPatientDbService, PatientDapperDbService>();
			services.AddHealthChecks().AddNpgSql(Configuration.GetConnectionString("PatientDbContext"));
		}

		private void SetupEfCoreORM(IServiceCollection services)
		{
			services.AddDbContext<PatientDbContext>(options =>
				options.UseNpgsql(Configuration.GetConnectionString("PatientDbContext")));
			services.AddScoped<IPatientDbService, PatientEfCoreDbService>();
			services.AddHealthChecks().AddDbContextCheck<PatientDbContext>();
		}

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHttpClient();
			services.AddControllers();
			services.AddSwaggerDocument();

			switch (Configuration.GetValue<int>("PATIENTSERVICE_ORM")) {
			case 2:
				SetupDapperORM(services);
				break;
			default:
				SetupEfCoreORM(services);
				break;
			}

			// TODO: Add health check monitoring.
			// TODO: Implement custom health check publisher to log critical errors.
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/error");
				app.UseHsts();
	    		app.UseHttpsRedirection();
			}

			app.UseRouting();
			app.UseAuthorization();

			app.UseOpenApi();
		    app.UseSwaggerUi3();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHealthChecks("/health");
			});
		}
	}
}
