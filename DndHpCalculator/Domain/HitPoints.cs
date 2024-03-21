using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public record HitPoints(uint Max, CharacterSheet CharacterSheet)
{
    private readonly List<HpModifier> _hpModifiers = [];
    public uint Max { get; } = Max;

    public uint Current
    {
        get
        {
            var step = new HpModifier.HpModifierInnerCalculationStep(Max, 0, Max, CharacterSheet.Defenses);
            foreach (var hpModifier in _hpModifiers) step = hpModifier.ModifyLifePool(step);
            return step.Current + step.Temp;
        }
    }

    public IReadOnlyList<HpModifier> HpModifiers => _hpModifiers;

    public void AddHpModifier(HpModifier hpModifier)
    {
        // Assign an Id to the HpModifier
        if (hpModifier.Id.IsTemporary) hpModifier.Id = Id.NewTimestampedId();
        _hpModifiers.Add(hpModifier);
    }

    public bool RemoveHpModifier(Id id)
    {
        var deletedCount = _hpModifiers.RemoveAll(m => m.Id.Equals(id));
        return deletedCount > 0;
    }

    public void ReplaceModifier(Id modId, HpModifier hpModifier)
    {
        //find old mod, replace go with new on but keep the same id
        var mod = _hpModifiers.Find(mod => mod.Id.Equals(modId));
        if (mod != null)
        {
            var index = _hpModifiers.IndexOf(mod);
            hpModifier.Id = modId;
            _hpModifiers[index] = hpModifier;
        }
    }
}