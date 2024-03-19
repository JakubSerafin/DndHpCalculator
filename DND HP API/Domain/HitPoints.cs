using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public record HitPoints(int Max)
{
    public int Max { get; } = Max;

    public int Current
    {
        get
        {
            var step = new HpModifier.HpModifierInnerCalculationStep(Max, 0, Max);
            foreach (var hpModifier in _hpModifiers)
            {
                step = hpModifier.ModifyLifePool(step);
            }
            return step.Current + step.Temp;
        }
    }
    
    private readonly List<HpModifier> _hpModifiers = [];

    public IReadOnlyList<HpModifier> HpModifiers => _hpModifiers;

    public void AddHpModifier(HpModifier hpModifier)
    {
        // Assign an Id to the HpModifier
        hpModifier.Id = Id.NewTemporaryId(_hpModifiers.Count + 1);
        _hpModifiers.Add(hpModifier);
    }

    public bool RemoveHpModifier(int id)
    {
        var deletedCount = _hpModifiers.RemoveAll(m => m.Id.Value == id);
        return deletedCount > 0;
    }
}