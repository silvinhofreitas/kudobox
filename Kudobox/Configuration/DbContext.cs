using Kudobox.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kudobox.Helpers.Constants;

namespace Kudobox.Configuration
{
    public static class DbContext
    {
        public static IServiceCollection ConfigureDbContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDbContext<UserContext>(o =>
                    o.UseSqlServer(configuration.GetConnectionString(ConfigurationConstants.CONNECTION_NAME)))
                .AddDbContext<CardContext>(o =>
                    o.UseSqlServer(configuration.GetConnectionString(ConfigurationConstants.CONNECTION_NAME)));

            return services;
        }
    }
}