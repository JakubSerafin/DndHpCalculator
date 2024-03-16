using DND_HP_API.Common;

namespace DND_HP_API.HpCalculator;

public interface IHpModifierRepository : IRepository<HpModifierModel>;

internal class HpModifierRepository : IHpModifierRepository
{
    private readonly List<HpModifierModel> _modifiers = new();

    public ICollection<HpModifierModel> GetAll()
    {
        return _modifiers;
    }

    public HpModifierModel? Get(int id)
    {
        return _modifiers.FirstOrDefault(m => m.Id == id);
    }

    public void Add(HpModifierModel item)
    {
        item.Id = _modifiers.Count + 1;
        _modifiers.Add(item);
    }

    public bool Delete(int id)
    {
        int removedCount = _modifiers.RemoveAll(m => m.Id == id);
        return removedCount > 0;
    }
}