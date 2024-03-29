﻿using Draftor.Core.Interfaces;

namespace Draftor.Core.Mapping;

public class Mapper : IMapper
{
    private readonly IDictionary<Tuple<Type, Type>, Func<object, object>> _maps;

    public Mapper()
    {
        _maps = new Dictionary<Tuple<Type, Type>, Func<object, object>>();
    }

    public void RegisterMap<TFrom, TTo>(Func<TFrom, TTo> mapFunc)
    {
        _maps.Add(new Tuple<Type, Type>(typeof(TFrom), typeof(TTo)), Mapper.CreateIndirectDelegate(mapFunc));
    }

    public T MapTo<T>(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        return MapNotNull<T>(obj);
    }

    public T? TryMapTo<T>(object? obj)
    {
        if (obj is null)
        {
            return default;
        }
        return MapNotNull<T>(obj);
    }

    private T MapNotNull<T>(object obj)
    {
        var typeKey = new Tuple<Type, Type>(obj.GetType(), typeof(T));
        if (!_maps.TryGetValue(typeKey, out Func<object, object>? mapDelegate))
            throw new MapDoesNotExistException($"Map from type {typeKey.Item1} to type {typeKey.Item2} was not registered.");
        try
        {
            var mappedObj = (T)mapDelegate(obj);
            return mappedObj;
        }
        catch (Exception ex)
        {
            throw new MappingFunctionException("Unexpected exception reported by mapping delegate. See inner details.", ex);
        }
    }

    private static Func<object, object> CreateIndirectDelegate<TFrom, TTo>(Func<TFrom, TTo> mapFunc)
    {
        object indirectDelegate(object obj) => mapFunc((TFrom)obj)!;
        return indirectDelegate;
    }
}