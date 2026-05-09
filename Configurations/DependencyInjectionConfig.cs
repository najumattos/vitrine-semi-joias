using System.Reflection;

namespace VitrineSemiJoias.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddSmartServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var serviceTypes = assembly.GetTypes()
            .Where(t => (t.Name.EndsWith("Service") || t.Name.EndsWith("Repository"))
                && !t.IsInterface && !t.IsAbstract);

        foreach (var serviceType in serviceTypes)
        {
           var interfaceType = serviceType.GetInterface($"I{serviceType.Name}");

            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, serviceType);
            }
            else
            {
                services.AddScoped(serviceType);
            }
        }

        return services;
    }
}
