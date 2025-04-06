using Accountater.Domain.Models;
using Accountater.Domain.Services;
using Accountater.Persistence.SqlServer;
using Accountater.Persistence.SqlServer.Services;
using Accountater.WebApp.ModelBinders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var domainAssemblies = new[] { typeof(FinancialTransaction).Assembly };

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(domainAssemblies);
});

builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new IEnumerableModelBinderProvider());
    options.ModelBinderProviders.Insert(0, new StronglyTypedIdModelBinderProvider());
});

builder.Services.AddTransient<IFinancialTransactionCsvParser, FinancialTransactionCsvParser>();
builder.Services.AddTransient<IFinancialTransactionRepository, SqlServerFinancialTransactionRepository>();
builder.Services.AddTransient<ITagRuleRepository, SqlServerTagRuleRepository>();
builder.Services.AddTransient<IAccountRepository, SqlServerAccountRepository>();
builder.Services.AddTransient<ICsvImportSchemaRepository, SqlServerCsvImportSchemaRepository>();
builder.Services.AddTransient<ICsvImportSchemaInfoReader, SqlServerCsvImportSchemaRepository>();
builder.Services.AddTransient<IRuleEvaluator, JintRuleEvaluator>();
builder.Services.AddTransient<IMonthlySpendingAnalyzer, SqlServerMonthlySpendingAnalyzer>();

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
