using Microsoft.AspNetCore.Routing;

namespace Slices.Endpoints.Mapper;
/// <summary>
/// Interface for declaring not static/ nested classes with endpoints
/// </summary>
public interface IHasEndpoints
{
    void Map(IEndpointRouteBuilder app);
}