using DND_HP_API.Domain;

namespace DND_HP_API.Common;

public interface IRepository<T>
{
    ICollection<T> GetAll();
    T? Get(int id);
    Id Add(T item);
    bool Delete(int id);
    
    
}