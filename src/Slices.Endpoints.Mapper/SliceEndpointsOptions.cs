using System.Reflection;

namespace Slices.Endpoints.Mapper;

public class SliceEndpointsOptions
{
    private Assembly? assembly = null;

    public SliceEndpointsOptions()
    {
        
    }

    public void RegisterEndpointsFromAssembly(Assembly assembly)
    {
        this.assembly = assembly;
    }

    internal Assembly Assembly => assembly ?? throw new InvalidOperationException("Assembly which contains the endpoints should be configured from application builder.") ;
}