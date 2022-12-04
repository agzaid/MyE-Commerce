using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Repo.Repository;
using Services.Shop;
using Services.Shop.CategoryRepo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Injection
{
    public static class ExtensionMethod
    {
        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }

        public static IServiceCollection LocalizationOptions(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(
        options =>
        {
            var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ar")
                };

            options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
        });
            return services;
        }
    }
}
