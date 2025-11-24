using MduiBlazor;
using MduiBlazor.Shared.Data;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<MaterialIconList>();

builder.Services.AddMduiBlazor();

await builder.Build().RunAsync();
