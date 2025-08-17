using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using S.Player.Utils.Options;

namespace S.Player.Utils.Extensions;

public static class ServiceCollectionExtensions
{
    public static Task ConfigureWritable<T>(this IServiceCollection services, IConfigurationSection section,
        string file = "appsettings.json") where T : class, new()
    {
        services.Configure<T>(section);

        services.AddTransient<IWritableOptions<T>>(provider =>
        {
            var environment = provider.GetService<IHostEnvironment>();
            var options = provider.GetService<IOptionsMonitor<T>>();
            if (environment != null)
            {
                if (options != null)
                {
                    return new WritableOptions<T>(environment, options, section.Key, file);
                }
            }

            return null!;
        });
        return Task.CompletedTask;
    }
}