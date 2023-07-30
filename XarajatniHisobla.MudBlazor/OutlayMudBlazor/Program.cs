using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using OutlayMudBlazor;
using OutlayMudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<OutlayService>();
builder.Services.AddScoped<DebtService>();
builder.Services.AddScoped<AccountService>();

builder.Services.AddSingleton<Utilities.ILocalStorage, Utilities.LocalStorage>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var configuration = builder.Configuration.Build();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(configuration["BaseAddress"]) });

builder.Services.AddMudServices();

await builder.Build().RunAsync();
