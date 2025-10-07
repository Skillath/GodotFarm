using System;
using Core.DependencyInjection.ServiceResolver;
using Core.Godot.MVVM;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace RealFriendlyFarm.Assets;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddTransient<TView>(
        this IServiceCollection serviceCollection,
        PackedScene template)
        where TView : Node
    {
        return serviceCollection.AddTransient<TView>(services => CreateNode<TView>(services, template));
    }
    
    public static IServiceCollection AddTransient<TIView, TView>(
        this IServiceCollection serviceCollection,
        PackedScene template)
        where TView : TIView
        where TIView : Node
    {
        
        return serviceCollection.AddTransient<TIView, TView>(services => CreateNode<TView>(services, template));
    }
    
    public static IServiceCollection AddKeyedTransient<TView>(
        this IServiceCollection serviceCollection,
        PackedScene template,
        object? key)
        where TView : Node
    {
        return serviceCollection.AddKeyedTransient<TView>(key, (services, _) => CreateNode<TView>(services, template));
    }
    
    public static IServiceCollection AddKeyedTransient<TIView, TView>(
        this IServiceCollection serviceCollection,
        PackedScene template,
        object? key)
        where TView : TIView
        where TIView : Node
    {
        
        return serviceCollection.AddKeyedTransient<TIView, TView>(key, (services, _) => CreateNode<TView>(services, template));
    }
    
    public static IServiceCollection AddScoped<TView>(
        this IServiceCollection serviceCollection,
        PackedScene template)
        where TView : Node
    {
        return serviceCollection.AddScoped<TView>(services => CreateNode<TView>(services, template));
    }
    
    public static IServiceCollection AddScoped<TIView, TView>(
        this IServiceCollection serviceCollection,
        PackedScene template)
        where TView : TIView
        where TIView : Node
    {
        
        return serviceCollection.AddScoped<TIView, TView>(services => CreateNode<TView>(services, template));
    }
    
    public static IServiceCollection AddKeyedScoped<TView>(
        this IServiceCollection serviceCollection,
        PackedScene template,
        object? key)
        where TView : Node
    {
        return serviceCollection.AddKeyedScoped<TView>(key, (services, _) => CreateNode<TView>(services, template));
    }
    
    public static IServiceCollection AddKeyedScoped<TIView, TView>(
        this IServiceCollection serviceCollection,
        PackedScene template,
        object? key)
        where TView : TIView
        where TIView : Node
    {
        
        return serviceCollection.AddKeyedScoped<TIView, TView>(key, (services, _) => CreateNode<TView>(services, template));
    }
    
    public static IServiceCollection AddSingleton<TIView, TView>(
        this IServiceCollection serviceCollection,
        PackedScene template)
        where TView : TIView
        where TIView : Node
    {
        
        return serviceCollection.AddSingleton<TIView, TView>(services => CreateNode<TView>(services, template));
    }
    
    public static IServiceCollection AddSingleton<TView>(
        this IServiceCollection serviceCollection,
        PackedScene template)
        where TView : Node
    {
        return serviceCollection.AddSingleton<TView>(services => CreateNode<TView>(services, template));
    }
    
    public static IServiceCollection AddKeyedSingleton<TIView, TView>(
        this IServiceCollection serviceCollection,
        PackedScene template,
        object? key)
        where TView : TIView
        where TIView : Node
    {
        
        return serviceCollection.AddKeyedSingleton<TIView, TView>(key, (services, _) => CreateNode<TView>(services, template));
    }
    
    public static IServiceCollection AddKeyedSingleton<TView>(
        this IServiceCollection serviceCollection,
        PackedScene template,
        object? key)
        where TView : Node
    {
        return serviceCollection.AddKeyedSingleton<TView>(key, (services, _) => CreateNode<TView>(services, template));
    }

    private static TView CreateNode<TView>(
        IServiceProvider services, 
        PackedScene template)
        where TView : Node
    {
        var resolver = services.GetRequiredService<IServiceResolver>();
        var root = services.GetRequiredService<Node>();
        return template.InstantiateAndResolve<TView>(resolver, root);
    }
}