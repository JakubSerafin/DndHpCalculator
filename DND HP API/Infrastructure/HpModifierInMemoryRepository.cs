using DND_HP_API.Domain;
using DND_HP_API.Domain.Abstract;
using DND_HP_API.Domain.Repositories;

namespace DND_HP_API.Infrastructure;

internal class HpModifierInMemoryRepository : IHpModifierRepository
{
    private readonly List<HpModifier> _modifiers = new();

    public ICollection<HpModifier> GetAll()
    {
        return _modifiers;
    }

    public HpModifier? Get(int id)
    {
        return _modifiers.FirstOrDefault(m => m.Id.Value == id);
    }

    public Id Add(HpModifier item)
    {
        if (item.Id.IsTemporary)
        {
            item.Id =  new Id(_modifiers.Count + 1);
            _modifiers.Add(item);
        }
        return item.Id;
    }

    public bool Delete(int id)
    {
        int removedCount = _modifiers.RemoveAll(m => m.Id.Value == id);
        return removedCount > 0;
    }
}