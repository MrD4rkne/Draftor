namespace Draftor.Core.Interfaces;

public interface IMapper
{
    /// <summary>
    /// Map object to T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">object mapped from, cannot be null</param>
    /// <returns></returns>
    T MapTo<T>(object obj);

    /// <summary>
    /// Map object to T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj">object mapped from</param>
    /// <returns>return null if obj was null, otherwise mapped</returns>
    T? TryMapTo<T>(object? obj);

    void RegisterMap<TFrom, TTo>(Func<TFrom, TTo> mapFunc);
}