using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SD.Application.Movies;
using SD.Persistence.Repositories.DBContext;
using SD.Persistence.Extensions;
using SD.Application.Extensions;
using SD.Web.Data;
using System.Reflection;
using SD.Resources.Attributes;
using SD.Resources;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var MovieDbConnectionString = builder.Configuration.GetConnectionString("MovieDbContext") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseSqlServer(MovieDbConnectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.RegisterRepositories();
builder.Services.RegisterApplicationServices();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MovieQueryHandler).GetTypeInfo().Assembly));

/* Möglichkeit für Speichern in serverseitigen Sessions hinzufügen*/
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

/* Browser-Spracherkennung implementieren */
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
                {
                    new("de"),
                    new("de-AT"),
                    new("de-DE"),
                    new("de-CH"),
                    new("en")
                };

    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("de");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddRazorPages()
                .AddRazorRuntimeCompilation();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

/* Damit ich Sessions auch tatsächlich verwenden kann */
app.UseSession();

/* Browser-Spracherkennung aktivieren */
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}");
app.MapRazorPages();

LocalizedDescriptionAttribute.Setup(new System.Resources.ResourceManager(typeof(BasicRes)));

app.Run();
