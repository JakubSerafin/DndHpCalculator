using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain.HpModifiers;

public class HealHpModifier: HpModifier
{
    public override HpModifierInnerCalculationStep ModifyLifePool(HpModifierInnerCalculationStep step)
    {
        var calculated = step with {Current = Math.Min(step.Current + Value, step.Max)};
        return calculated;
    }
}