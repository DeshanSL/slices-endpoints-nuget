using Slices.Endpoints.Mapper;

namespace Microsoft.AspNetCore.Builder;
/// <summary>
/// Contains WebApplication builder extension
/// </summary>
public static class SlicesEndpointMapperExtensions
{
    /// <summary>
    /// Register Executing assembly to find endpoints
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configurations"></param>
    public static void UseSliceEndpoints(this WebApplication app, Action<SliceEndpointsOptions> configurations)
    {
        var options = new SliceEndpointsOptions();
        configurations(options);
        var assembly = options.Assembly;
        Mapper.MapEndpointsNestedClasses(assembly,app);
        Mapper.MapIHasEndpointsImplementations(assembly, app);
    }
}