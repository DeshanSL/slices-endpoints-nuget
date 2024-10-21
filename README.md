# slices-endpoints-nuget
This NuGet package simplifies the implementation of minimal APIs within nested, non-static classes, making it an ideal choice for organizing endpoints in vertical slice architecture projects. It provides a clean, modular structure that enhances code organization by grouping related endpoints within feature-based classes.

## Start services

```csharp
app.UseSliceEndpoints(options =>
{
      options.RegisterEndpointsFromAssembly(typeof(Program).Assembly);
});
```

# Implementations

## Using IHasEndpoints interface

Being nested or implementing IFeature interface is not mandatory when using IHasEndpoints

```csharp
public class CreateOrder : IFeature
{
    public record Command() : IRequest<Return<OrderId>>
    {
    };

    internal sealed class Handler : IRequestHandler<Command, Return<OrderId>>
    {
        private readonly IAppEventStore _store;

        public Handler(IAppEventStore store)
        {
            _store = store;
        }
        public async Task<Return<OrderId>> Handle(Command request, CancellationToken cancellationToken)
        {
           // Handler logic
        }
    }
    // THIS ENDPOINT WILL BE MAPPED TO THE ROUTER BY THE PACKAGE
    public class Endpoint : IHasEndpoints
    {
        public record Request()
        {
        }
        public void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("api/orders", async ([FromBody] Request request, [FromServices] IMediator mediator) =>
            {
                var result = await mediator.Send(new Command());
                return result.Value;
            });
        }
    }

}
```

## Using naming convention

 MANDATORY to implement IFeature marker interface for declaring class/type of "Endpoints"

```csharp
// Implementing IFeature is MUST to use naming convention.
public class CreateOrder : IFeature
{
    // Nested class should be named Endpoints
    public class Endpoints
    {
        public record Request()
        {
        }
        // Map method SHOULD accept IEndpointRouteBuilder
        public void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("api/orders", async ([FromBody] Request request, [FromServices] IMediator mediator) =>
            {
                var result = await mediator.Send(new Command());
                return result.Value;
            });
        }
    }
}

```
