namespace Draftor.Abstract;

public interface IMapper
{
    T MapTo<T>(object obj);

    void RegisterMap<TFrom, TTo>(Func<TFrom, TTo> mapFunc);
}