using DND_HP_API.Domain;

namespace DND_HP_API.HpCalculator;

public class HpModifierModel
{
    public int? Id { get; set; }
    public int Value { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }

    public HpModifier BuildEntity()
    {
        return new HpModifier
        {
            Id = Id.HasValue? new Id(Id.Value) : Domain.Id.NewTemporaryId(),
            Value = Value,
            Type = Type=="Damage"? HpModifierType.Damage : HpModifierType.Healing,
        };
    }

    public static HpModifierModel FromEntity(HpModifier modifier)
    {
        return new HpModifierModel
        {
            Id = modifier.Id.Value,
            Value = modifier.Value,
            Type = modifier.Type.ToString(),
            Description = "Test"
        };
    }
}