namespace DND_HP_API.Domain.Abstract;

public abstract class HpModifier: Entity
{
    public record HpModifierInnerCalculationStep(int Current, int Temp, int Max);
    public int Value { get; set; }

    public abstract HpModifierInnerCalculationStep ModifyLifePool(HpModifierInnerCalculationStep step);
}