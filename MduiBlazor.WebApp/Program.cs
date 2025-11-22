using MduiBlazor;
using MduiBlazor.Shared.Data;
using MduiBlazor.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<MaterialIconList>();
builder.Services.AddHttpClient();

builder.Services.AddMduiBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MduiBlazor.Client._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(MduiBlazor.Shared._Imports).Assembly);

app.Run();
