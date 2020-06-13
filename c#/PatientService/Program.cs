using System;
using System.Configuration;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PatientService.Models;
using PatientService.Services;

namespace PatientService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

			using (IServiceScope serviceScope = host.Services.CreateScope()) {
				IServiceProvider serviceProvider = serviceScope.ServiceProvider;

				ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();
				IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
				IPatientDbService patientDbService = serviceProvider.GetRequiredService<IPatientDbService>();
				IWebHostEnvironment webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

				int retires = 0;
				bool connected = false;
				while (!connected) {
					try {
						connected = patientDbService.CanConnect();
					} catch (DbServiceException e) {
						logger.LogError("Cannot connect to database. Retying connection.");
						//System.Threading.Thread.Sleep(5000);
						if(retires > 3) {
							logger.LogCritical("An error occured testing the database connection.");
							throw e;
						} else {
							retires++; 
							continue;
						}
					}
				}

				switch (configuration["ORM"]) {
				case "EfCore":
					logger.LogInformation("Entity Framework Core ORM chosen.");
					break;
				case "Dapper":
					logger.LogInformation("Dapper ORM chosen.");
					break;
				default:
					logger.LogWarning("No/invalid ORM chosen. Defaulting to Entity Framework Core ORM.");
					break;
				}

				if (webHostEnvironment.IsDevelopment()) {
					logger.LogInformation("Creating database.");

					try {
						patientDbService.EnsureDeleted();
						patientDbService.EnsureCreated();
					} catch (DbServiceException e) {
						logger.LogCritical("An error occured recreating the database.");
						throw e;
					}

					logger.LogInformation("Seeding database.");

					if (string.IsNullOrEmpty(configuration["SeedDatabase"])) {
						logger.LogWarning("SeedDatabase configuration not found/empty. Skipping database seeding.");
					} else if (bool.Parse(configuration["SeedDatabase"])) {
						try {
							SeedData.Initialize(serviceProvider);
						} catch (Exception e) {
							logger.LogCritical("An error occured seeding the database.");
							throw e;
						}
					} else {
						logger.LogInformation("Skipping database seeding.");
					}
				}
			}

			host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
