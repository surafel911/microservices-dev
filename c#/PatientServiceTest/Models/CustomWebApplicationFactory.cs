using System;
using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using PatientService.Data;
using PatientService.Services;
using DataAtThePointOfCare.Models;
using DataAtThePointOfCare.Services;

namespace PatientServiceTest.Models
{
    /*
     * TODO: Determine whether a custom WebAppFactory is required to test Postgresql DBMS calls
     * 		If so...
     * 		TODO: Modify customer WebAppFactory to use Postgresql DBMS when Dapper is selected.
     *      TODO: Otherwise, continue using the in memory db.
     *
     * TODO: Get integration testing w/ Dapper ORM working.
     */
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        private void 
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                ServiceDescriptor descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<PatientDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<PatientDbContext>(options => {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
                
                ServiceProvider serviceProvider = services.BuildServiceProvider();

                IConfiguration configuration =  serviceProvider.GetRequiredService<IConfiguration>();

                switch (configuration.GetValue<int>("PATIENTSERVICE_ORM")) {
                    case 2:
                        services.AddScoped<IPatientDbService, PatientDapperDbService>();
                        break;
                    default:
                        services.AddScoped<IPatientDbService, PatientEfCoreDbService>();
                        break;
                }

                using (IServiceScope scope = serviceProvider.CreateScope()) {
                    IServiceProvider scopedProvider = scope.ServiceProvider;
                    PatientDbContext patientDbContext = scopedProvider.GetRequiredService<PatientDbContext>();
                    IPatientDbService patientDbService = scopedProvider.GetRequiredService<IPatientDbService>();
                    ILogger<CustomWebApplicationFactory<TStartup>> logger = scopedProvider
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    patientDbContext.Database.EnsureDeleted();
                    patientDbContext.Database.EnsureCreated();

                    try {
                        TestUtilities.InitializeDb(patientDbService);
                    } catch (Exception ex) {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}