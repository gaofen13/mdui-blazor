using MduiBlazor;
using MduiBlazor.Shared;
using MduiBlazor.Shared.Data;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<MaterialIconList>();

builder.Services.AddMduiBlazor();

await builder.Build().RunAsync();
