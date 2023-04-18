using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Repo.Repository;
using Services.Cashier;
using Services.Shop;
using Services.Shop.CategoryRepo;
using Services.Shop.CustomerRepo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Injection
{
    public static class DependencyInjection
    {
        public static IServiceCollection IdentityConfiguration(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                //options.Password.RequiredLength = 3;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;

            });
            services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/Identity/Login";
                option.AccessDeniedPath = "/Identity/AccessDenied";
                option.LogoutPath = "/Identity/Logout";
                option.ExpireTimeSpan = TimeSpan.FromDays(1);
            });
            return services;
        }
        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISkuMainItemService, SkuMainItemService>();
            services.AddScoped<ISkuSubItemService, SkuSubItemService>();

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
