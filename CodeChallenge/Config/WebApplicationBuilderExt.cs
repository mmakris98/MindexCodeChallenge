using CodeChallenge.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeChallenge.Config
{
    public static class WebApplicationBuilderExt
    {
        private static readonly string EmployeeDB_NAME = "EmployeeDB";
        private static readonly string CompensationDB_NAME = "CompensationDB";
        public static void UseEmployeeDB(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseInMemoryDatabase(EmployeeDB_NAME);
            });
            builder.Services.AddDbContext<CompensationContext>(options =>
            {
                options.UseInMemoryDatabase(CompensationDB_NAME);
            });
        }
    }
}
