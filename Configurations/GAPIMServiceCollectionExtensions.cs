using System.Reflection;
using GAPIM_GenericAPIMauricio.DataBase;
using GAPIM_GenericAPIMauricio.Interfaces;
using GAPIM_GenericAPIMauricio.Repositories;
using GAPIM_GenericAPIMauricio.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GAPIM_GenericAPIMauricio.Configurations;

public static class GAPIMServiceCollectionExtensions
{
    public static IGAPIMBuilder AddGAPIM(this IServiceCollection services, Action<GAPIMCustom>? setupAction = null)
    {
        // ==========================================
        // 1. Injeccao de dependencia
        // ==========================================
        services.AddSingleton<MemoryContext>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryMemory<>));
        services.AddScoped(typeof(IGenericService<,,>), typeof(GenericService<,,>));
        
        // ==========================================
        // 2. AutoMapper
        // ==========================================
        var apiAssembly = Assembly.GetEntryAssembly();
        var libraryAssembly = typeof(GAPIMServiceCollectionExtensions).Assembly;

        var assembliesToScan = new List<Assembly> { libraryAssembly };
        if (apiAssembly != null && apiAssembly != libraryAssembly)
        {
            assembliesToScan.Add(apiAssembly);
        }

        services.AddAutoMapper(cfg => {}, assembliesToScan.ToArray());
        
        // ==========================================
        // 3. Swagger / OpenAPI
        // ==========================================
        var options = new GAPIMCustom();
        setupAction?.Invoke(options);
        
        services.Configure(setupAction ?? (opt => { })); 

        services.AddOpenApi(options.Version, apiOptions =>
        {
            apiOptions.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info.Title = options.Name;
                document.Info.Description = options.Description;
                document.Info.Version = options.Version;
                
                return Task.CompletedTask;
            });
        });
        
        services.AddEndpointsApiExplorer();
        
        return new GAPIMBuilder(services);
    }
}