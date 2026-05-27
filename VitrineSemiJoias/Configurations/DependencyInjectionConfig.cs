using System.Reflection;

namespace VitrineSemiJoias.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddSmartServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var implementationTypes = assembly.GetTypes()
            .Where(t => (t.Name.EndsWith("Service") || t.Name.EndsWith("Repository"))
                && !t.IsInterface && !t.IsAbstract);

        foreach (var type in implementationTypes)
        {
            // Pega todas as interfaces que a classe implementa
            var interfaces = type.GetInterfaces();

            // Tenta encontrar a interface que segue o padrão I + NomeDaClasse
            var mainInterface = interfaces.FirstOrDefault(i => i.Name == $"I{type.Name}");

            if (mainInterface != null)
            {
                services.AddScoped(mainInterface, type);
            }
            else
            {
                // Se não achar a interface padrão, mas a classe implementa alguma interface, 
                // você pode decidir se registra a primeira ou apenas a classe
                services.AddScoped(type);
            }
        }

        return services;
    }
}
