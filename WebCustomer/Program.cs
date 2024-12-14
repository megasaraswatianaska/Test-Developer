using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Syncfusion.Blazor;
using WebCustomer.Models;
using WebCustomer.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(connectionString: configuration.GetConnectionString("DBConnection"));
});


builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = false; });
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTQ1NTA4QDMxMzkyZTMzMmUzMEdOM291QzNGNGcwTHR1SllmbkJxaWo1NitCSlR1dmVHaW1UcE5YbTRVNWM9");

#region Services
builder.Services.AddScoped<IDataSalesServices, DataSalesServices>();
#endregion
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
