namespace DND_HP_API.Common;

public interface IRepository<T>
{
    ICollection<T> GetAll();
    T? Get(int id);
    void Add(T item);
    bool Delete(int id);
}