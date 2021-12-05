using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace Kudobox.Configuration
{
    public static class ResourcesConfiguration
    {
        public static void ConfigureResources(this IServiceCollection services)
        {
            services.AddLocalization(o => o.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new("en"),
                    new("es"),
                    new("pt-BR")
                };

                options.SetDefaultCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }
    }
}