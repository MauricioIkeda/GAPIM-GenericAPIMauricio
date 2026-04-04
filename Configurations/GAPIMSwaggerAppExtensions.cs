using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GAPIM_GenericAPIMauricio.Configurations;

public static class GAPIMSwaggerAppExtensions
{
    public static WebApplication UseGAPIMSwagger(this WebApplication app)
    {
        var config = app.Services.GetRequiredService<IOptions<GAPIMCustom>>().Value;
        
        app.MapOpenApi();

        app.UseSwaggerUI(swaggerOpt =>
        {
            swaggerOpt.SwaggerEndpoint($"/openapi/{config.Version}.json", config.Name);
            swaggerOpt.RoutePrefix = string.Empty;
        });
        
        return app;
    }
}