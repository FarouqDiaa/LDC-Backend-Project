using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.InfrastructureLayer;
using Microsoft.EntityFrameworkCore;

namespace Project.RuntimeLayer
{

    public static class DbConnection
    {
        public static void ConfigureDbConnectionServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DbConnectionString"));
            });

        }
    }
}
