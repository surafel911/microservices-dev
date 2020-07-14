using System;
using System.Net.Http;
using System.Data.Common;
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
		private static int MaxRetries = 5;

		private static void LogOrmConfig(
			ILogger<Program> logger,
			IConfiguration configuration)
		{
			switch (ConfigurationBinder.GetValue<int>(configuration, "PATIENTSERVICE_ORM")) {
			case 1:
				logger.LogInformation("Entity Framework Core ORM chosen.");
				break;
			case 2:
				logger.LogInformation("Dapper ORM chosen.");
				break;
			default:
				logger.LogWarning("Defaulting to Entity Framework Core ORM.");
				break;
			}
		}

		private static void EnsureDbCreated(
			ILogger<Program> logger,
			IPatientDbService patientDbService,
			IWebHostEnvironment webHostEnvironment)
		{
			int retries = 0;
			bool created = false;

			if (webHostEnvironment.IsDevelopment()) {
				logger.LogInformation("Creating new database.");

				do {
					try {
						patientDbService.EnsureDeleted();
						patientDbService.EnsureCreated();

						created = true;
					} catch (Exception e) {
						if (retries < MaxRetries) {
							logger.LogError(e, "Connection to DBMS failed. Retrying connection.");
							System.Threading.Thread.Sleep(5000);

							retries++;
						} else {
							logger.LogCritical(e, "Failed to connect to create database.");
							throw;
						}
					}
				} while (!created);
			} else  {
				if (!patientDbService.CanConnect()) {
					logger.LogInformation("Cannot connect to database, Creating new database.");

					do {
						try {
							patientDbService.EnsureCreated();
						} catch (Exception e) {
							if (retries < MaxRetries) {
								logger.LogError(e, "Connection to DBMS failed. Retrying connection.");
								System.Threading.Thread.Sleep(5000);

								retries++;
							} else {
								logger.LogCritical(e, "Failed to connect to create database.");
								throw;
							}
						}
					} while (!created);
				}
			}
		}

		private static void TestDbConnection(
			ILogger<Program> logger,
			IPatientDbService patientDbService)
		{
			for (int i = 0; i < MaxRetries - 1; i++) {
				if (patientDbService.CanConnect()) {
					logger.LogInformation("Successfully connected to database.");
					return;
				}

				logger.LogError("Cannot connect to database. Retrying connection.");
				System.Threading.Thread.Sleep(5000);
			}

			if (patientDbService.CanConnect()) {
				logger.LogInformation("Successfully connected to database.");
				return;
			} else {
				logger.LogCritical("Failed to connect to database.");
			}
		}

		private static void SeedDb(
			ILogger<Program> logger,
			IConfiguration configuration,
			IServiceProvider serviceProvider,
			IPatientDbService patientDbService,
			IWebHostEnvironment webHostEnvironment)
		{
			bool seedDb = false;
			if (webHostEnvironment.IsDevelopment()) {
				if (Boolean.TryParse(configuration["PATIENTSERVICE_SEED_DB"], out seedDb) && seedDb) {
					logger.LogInformation("Seeding database.");

					SeedData.Initialize(patientDbService,
						serviceProvider.GetRequiredService<IHttpClientFactory>());
				}
			}
		}

        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

			using (IServiceScope serviceScope = host.Services.CreateScope()) {
				IServiceProvider serviceProvider = serviceScope.ServiceProvider;

				ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();
				IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
				IPatientDbService patientDbService = serviceProvider.GetRequiredService<IPatientDbService>();
				IWebHostEnvironment webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

				logger.LogInformation("Environment: " + webHostEnvironment.EnvironmentName);

				LogOrmConfig(logger, configuration);
				EnsureDbCreated(logger, patientDbService, webHostEnvironment);
				TestDbConnection(logger, patientDbService);
				SeedDb(logger, configuration, serviceProvider, patientDbService, webHostEnvironment);
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
