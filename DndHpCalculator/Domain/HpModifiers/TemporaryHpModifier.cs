using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain.HpModifiers;

public class TemporaryHpModifier : HpModifier
{
    public override HpModifierInnerCalculationStep ModifyLifePool(HpModifierInnerCalculationStep step)
    {
        var calculated = step with { Temp = Math.Max(step.Temp, Value) };
        return calculated;
    }
}