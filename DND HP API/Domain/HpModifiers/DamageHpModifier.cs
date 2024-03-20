using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain.HpModifiers;

public class DamageHpModifier: HpModifier
{
    public required DamageType DamageType { get; set; }
    public override HpModifierInnerCalculationStep ModifyLifePool(HpModifierInnerCalculationStep step)
    {
        var damage = step.characterSheetDefenses.Min(def=>def.Reduce(DamageType,Value));
        var remainingDamage = Math.Max(0, damage - step.Temp);
        return step with 
        {
            Temp = Math.Max(0, step.Temp - damage),
            Current = Math.Max(0, step.Current - remainingDamage)
        };
    }
}