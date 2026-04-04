using Microsoft.Extensions.DependencyInjection;

namespace GAPIM_GenericAPIMauricio.Configurations;

public interface IGAPIMBuilder
{
    IServiceCollection Services { get; }
}

internal class GAPIMBuilder : IGAPIMBuilder
{
    public IServiceCollection Services { get; }
    
    public GAPIMBuilder(IServiceCollection services)
    {
        Services = services;
    }
}