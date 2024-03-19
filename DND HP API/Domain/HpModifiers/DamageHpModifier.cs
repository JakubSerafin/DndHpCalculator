using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain.HpModifiers;

public class DamageHpModifier: HpModifier
{
    public override HpModifierInnerCalculationStep ModifyLifePool(HpModifierInnerCalculationStep step)
    {
        var remainingDamage = Math.Max(0, Value - step.Temp);
        return step with 
        {
            Temp = Math.Max(0, step.Temp - Value),
            Current = Math.Max(0, step.Current - remainingDamage)
        };
    }
}