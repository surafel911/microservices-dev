using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Npgsql;
using Microsoft.EntityFrameworkCore;

using PatientService.Data;

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
            services.AddControllers();

            services.AddDbContext<PatientServiceDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PatientServiceDbContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (PatientServiceDbContext PatientServiceDbContext = app.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<PatientServiceDbContext>())
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();

                    PatientServiceDbContext.Database.EnsureDeletedAsync();
                    PatientServiceDbContext.Database.EnsureCreatedAsync();
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
