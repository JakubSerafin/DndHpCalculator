using DND_HP_API.Domain;

namespace DND_HP_API.Controllers.ApiModels;

public class ModifierModel
{
    public required string AffectedObject { get; init; } // What the modifier affects (e.g., stats)
    public required string AffectedValue { get; init; } // Specific property to change (e.g., constitution)
    public int Value { get; init; } // The amount to change by

    internal static ModifierModel FromDomainEntity(Modifier argModifierModel)
    {
        return new ModifierModel
        {
            AffectedObject = argModifierModel.AffectedObject,
            AffectedValue = argModifierModel.AffectedValue,
            Value = argModifierModel.Value
        };
    }
}