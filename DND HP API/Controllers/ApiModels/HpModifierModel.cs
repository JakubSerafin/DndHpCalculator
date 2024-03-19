using DND_HP_API.Domain;
using DND_HP_API.Domain.Abstract;
using DND_HP_API.Domain.HpModifiers;

namespace DND_HP_API.Controllers.ApiModels;

public class HpModifierModel
{
    public int? Id { get; set; }
    public int Value { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }

    public HpModifier BuildEntity()
    {
        switch (Type)
        {
            case HpModifierTypesModel.Damage:
                return new DamageHpModifier
                {
                    Id = Id.HasValue ? new Id(Id.Value) : Domain.Abstract.Id.NewTemporaryId(),
                    Value = Value,
                };
            case HpModifierTypesModel.Healing:
                return new HealHpModifier
                {
                    Id = Id.HasValue ? new Id(Id.Value) : Domain.Abstract.Id.NewTemporaryId(),
                    Value = Value,
                };
            case HpModifierTypesModel.Temporary:
                return new TemporaryHpModifier
                {
                    Id = Id.HasValue ? new Id(Id.Value) : Domain.Abstract.Id.NewTemporaryId(),
                    Value = Value,
                };
            default:
                throw new InvalidOperationException($"Unsupported type: {Type}");
        }
    }

    public static HpModifierModel FromEntity(HpModifier modifier)
    {
        return new HpModifierModel
        {
            Id = modifier.Id.Value,
            Value = modifier.Value,
            Type = modifier switch
            {
                HealHpModifier => HpModifierTypesModel.Healing,
                DamageHpModifier => HpModifierTypesModel.Damage,
                TemporaryHpModifier => HpModifierTypesModel.Temporary,
                _ => throw new InvalidOperationException($"Unsupported type: {modifier.GetType()}")
            },
            Description = "Test"
        };
    }
}