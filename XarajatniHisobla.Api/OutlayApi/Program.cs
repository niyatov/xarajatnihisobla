using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OutlayApi;
using OutlayApi.Extensions;
using OutlayApi.Extentions;
using OutlayApi.Middleware;
using OutlayApi.Repositories;
using OutlayApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
});

builder.Services.AddIdentityManagers();

builder.Services.Configure<SettingOptions>(
    builder.Configuration.GetSection(SettingOptions.Setting));

var options = new SettingOptions();
builder.Configuration.Bind(SettingOptions.Setting, options);

builder.SerilogConfig(Options.Create(options));

builder.Services.AddCorsPolicy(Options.Create(options));

builder.Services.AddAppDbContext(Options.Create(options));

builder.Services.AddMemoryCache();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IOutlayService, OutlayService>();
builder.Services.AddTransient<IDebtService, DebtService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
