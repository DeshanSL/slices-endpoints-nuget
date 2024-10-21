using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace Slices.Endpoints.Mapper;

internal class Mapper
{
    /// <summary>
    /// Maps Endpoints class in IFeature implementations.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="app"></param>
    internal static void MapEndpointsNestedClasses(Assembly assembly, WebApplication app)
    {
        IEnumerable<Type> featureTypes = assembly.GetTypes()
            .Where(type => typeof(IFeature).IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

        foreach (var featureType in featureTypes)
        {
            Type? endpointsNestedType = featureType.GetNestedType("Endpoints", BindingFlags.Public);

            if (endpointsNestedType == null) continue;

            ConstructorInfo? constructor = endpointsNestedType.GetConstructor(Type.EmptyTypes);

            if (constructor == null) continue;

            var instance = GetInstance(constructor);

            MethodInfo? mapMethod = GetMapMethod(endpointsNestedType);

            if (mapMethod == null) continue;

            mapMethod.Invoke(instance, new object[] { app });
        }
    }
    /// <summary>
    /// Maps classes that has endpoints implements IHasEndpoints
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="app"></param>
    internal static void MapIHasEndpointsImplementations(Assembly assembly, WebApplication app)
    {
        var interfaceImplementations = assembly.GetTypes().Where(
            type => typeof(IHasEndpoints).IsAssignableFrom(type)
                    && type is { IsInterface: false, IsAbstract: false }
                    && !(type.IsNested && typeof(IFeature).IsAssignableFrom(type.DeclaringType) && type.Name == "Endpoints"));


        foreach (var interfaceImplementation in interfaceImplementations)
        {
            var constructor = interfaceImplementation.GetConstructor(Type.EmptyTypes);

            if (constructor == null) continue;

            var instance = GetInstance(constructor);

            var method = GetMapMethod(interfaceImplementation);

            if (method == null) continue;

            _ = method.Invoke(instance, new object?[] { app });
        }
    }

    private static MethodInfo? GetMapMethod(Type type)
    {
        return type.GetMethod("Map", BindingFlags.Public | BindingFlags.Instance);
    }

    private static object GetInstance(ConstructorInfo constructorInfo)
    {
        object instance = constructorInfo.Invoke(null);
        return instance;
    }
}
