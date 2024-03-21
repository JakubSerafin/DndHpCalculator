namespace DND_HP_API.Domain.Abstract;

public abstract class HpModifier : Entity
{
    public uint Value { get; set; }

    public abstract HpModifierInnerCalculationStep ModifyLifePool(HpModifierInnerCalculationStep step);

    public record HpModifierInnerCalculationStep(uint Current, uint Temp, uint Max, List<Defence> CharacterSheetDefenses);
}