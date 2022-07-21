using System;

using CodeChallenge.Data;
using CodeChallenge.Repositories;
using CodeChallenge.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeChallenge.Config
{
    public class App
    {
        public WebApplication Configure(string[] args)
        {
            args ??= Array.Empty<string>();

            var builder = WebApplication.CreateBuilder(args);

            builder.UseEmployeeDB();
            
            AddServices(builder.Services);

            var app = builder.Build();

            var env = builder.Environment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedEmployeeDB();
            }

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        private void AddServices(IServiceCollection services)
        {
            //DB Context
            services.AddDbContext<EmployeeContext>(options => options.UseInMemoryDatabase("EmployeeDB"));
            services.AddDbContext<DirectReportContext>(options => options.UseInMemoryDatabase("DirectReportDB"));
            services.AddDbContext<CompensationContext>(options => options.UseInMemoryDatabase("CompensationDB"));

            //Services
            services.AddScoped<IEmployeeService, EmployeeService>();

            //Repositories
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICompensationRepository, CompensationRepository>();
            services.AddScoped<IDirectReportRepository, DirectReportRepository>();

            services.AddControllers();
        }

        private void SeedEmployeeDB()
        {
            new EmployeeDataSeeder(
                new EmployeeContext(
                    new DbContextOptionsBuilder<EmployeeContext>().UseInMemoryDatabase("EmployeeDB").Options
                ),
                new DirectReportContext(
                    new DbContextOptionsBuilder<DirectReportContext>().UseInMemoryDatabase("DirectReportDB").Options
                ),
                new CompensationContext(
                    new DbContextOptionsBuilder<CompensationContext>().UseInMemoryDatabase("CompensationDB").Options
                )
            ).Seed().Wait();
        }
    }
}
