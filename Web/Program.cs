using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.EntityFrameworkCore;
using Repo;
using Services.Injection;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Services;
using Services.Injection.Localization;
using System.Reflection;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Identity;
using Data.Entities.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDBContext>(
    options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), builder =>
    {
        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });
});
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

builder.Services.IdentityConfiguration();

//builder.Services.AddSingleton<LocService>();
builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");
builder.Services.AddMvc().AddViewLocalization()
    .AddDataAnnotationsLocalization(op =>
    {
        op.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
            return factory.Create("SharedResource", assemblyName.Name);
        };
    });

builder.Services
        .LocalizationOptions()
        .InjectServices();

#region RequestLocalizationOptions
//builder.Services.Configure<RequestLocalizationOptions>(
//        options =>
//        {
//            var supportedCultures = new List<CultureInfo>
//                {
//                    new CultureInfo("en-US"),
//                    new CultureInfo("ar")
//                };

//            options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
//            options.SupportedCultures = supportedCultures;
//            options.SupportedUICultures = supportedCultures;

//            options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
//        });
#endregion

builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
});

await builder.Services.AddBackOfficeAppServicesConfiguration("KAJ.Web.BackOffice");

builder.Services.AddAutoMapper(typeof(Program).Assembly);

//builder.Services.InjectServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseRewriter(new RewriteOptions().Add(RewriteRules.RedirectRequests));
var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

app.MapControllerRoute(
    name: "Admin",
    pattern: "{culture:culture}/{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "LocalizedDefault",
    pattern: "{culture:culture}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
