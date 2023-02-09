using Microsoft.Extensions.DependencyInjection;

namespace MduiBlazor
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMduiBlazor(this IServiceCollection services)
        {
            return services.AddMduiSnackbar();
        }

        public static IServiceCollection AddMduiSnackbar(this IServiceCollection services) => services.AddScoped<SnackbarService>();
    }
}
