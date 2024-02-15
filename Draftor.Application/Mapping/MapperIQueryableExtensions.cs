using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draftor.Core.Interfaces;

namespace Draftor.Core.Mapping;
public static class MapperIQueryableExtensions
{
    /// <summary>
    /// Project all items to TTo by provided mapper.
    /// WARNING: objects cannot be null, otherwise will be thrown.
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    public static IQueryable<TTo> ProjectTo<TFrom, TTo>(this IQueryable<TFrom> queryable, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        return queryable.Select(x => mapper.MapTo<TTo>(x!));
    }
}
