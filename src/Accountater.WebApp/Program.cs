using Accountater.Domain.Commands;
using Accountater.Domain.Services;
using Accountater.Persistence.SqlServer;
using Accountater.Persistence.SqlServer.Services;
using Accountater.WebApp.ModelBinders;
using Accountater.WebApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var domainAssemblies = new[] { typeof(ImportCheckingTransactions).Assembly };

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(domainAssemblies);
});

builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new IdModelBinderProvider());
});

builder.Services.AddTransient<ICheckingTransactionCsvParser, CheckingTransactionCsvParser>();
builder.Services.AddTransient<ICreditTransactionCsvParser, CreditTransactionCsvParser>();
builder.Services.AddTransient<IFinancialTransactionRepository, SqlServerFinancialTransactionRepository>();

builder.Services.AddDbContext<AccountaterDbContext>(options =>
    options.UseSqlServer(builder.Configuration[$"connectionStrings:database"]));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
