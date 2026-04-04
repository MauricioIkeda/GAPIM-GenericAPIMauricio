using System.Collections.Concurrent;
using GAPIM_GenericAPIMauricio.Entities;

namespace GAPIM_GenericAPIMauricio.DataBase;

public class MemoryContext
{
    private readonly ConcurrentDictionary<Type, object> _database = new();

    public ConcurrentDictionary<Guid, T> Set<T>() where T : BaseEntity
    {
        var type = typeof(T);
        
        var table = _database.GetOrAdd(type, _ => new ConcurrentDictionary<Guid, T>());
        
        return (ConcurrentDictionary<Guid, T>)table;
    }
}