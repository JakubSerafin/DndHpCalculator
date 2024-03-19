using DND_HP_API.Domain;

namespace DND_HP_API.Controllers.ApiModels;

public class ModifierModel
{
    public string AffectedObject { get; set; } // What the modifier affects (e.g., stats)
    public string AffectedValue { get; set; } // Specific property to change (e.g., constitution)
    public int Value { get; set; } // The amount to change by

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