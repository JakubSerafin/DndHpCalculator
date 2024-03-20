using DND_HP_API.Domain;
using DND_HP_API.Domain.Abstract;
using DND_HP_API.Domain.HpModifiers;

namespace DND_HP_API.Controllers.ApiModels;

public class HpModifierModel
{
    public string? Id { get; set; }
    public int Value { get; set; }
    public string? Type { get; set; }
    public string? DamageType { get; set; }
    public string? Description { get; set; }

    public HpModifier BuildEntity()
    {
        switch (Type)
        {
            case HpModifierTypesModel.Damage:
                return new DamageHpModifier
                {
                    Id = GenerateId(),
                    Value = Value,
                    DamageType = DamageType != null ? 
                        DamageMapper.FromStringName(DamageType) :
                        Domain.DamageType.Bludgeoning
                };
            case HpModifierTypesModel.Healing:
                return new HealHpModifier
                {
                    Id = GenerateId(),
                    Value = Value,
                };
            case HpModifierTypesModel.Temporary:
                return new TemporaryHpModifier
                {
                    Id = GenerateId(),
                    Value = Value,
                };
            default:
                throw new InvalidOperationException($"Unsupported type: {Type}");
        }
    }

    private Id GenerateId()
    {
        return Id!=null ?  new Id(long.Parse(Id)): Domain.Abstract.Id.NewTemporaryId();
    }

    public static HpModifierModel FromEntity(HpModifier modifier)
    {
        return new HpModifierModel
        {
            Id = modifier.Id.Value.ToString(),
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