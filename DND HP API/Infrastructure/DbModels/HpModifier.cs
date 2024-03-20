using DND_HP_API.Controllers.ApiModels;
using DND_HP_API.Domain;
using DND_HP_API.Domain.Abstract;
using DND_HP_API.Domain.HpModifiers;

namespace DND_HP_API.Infrastructure.DbModels;

public class HpModifierDbModel
{
    public long Id { get; init; }
    public int Value { get; init; }
    public required string Type { get; init; }

    public string? DamageType { get; init; }
    public string? Description { get; set; }

    public static HpModifierDbModel FromDomainEntity(HpModifier arg)
    {
        return new HpModifierDbModel
        {
            Description = "temp",
            Id = !arg.Id.IsTemporary ? arg.Id.Value : DateTime.Now.Ticks,
            Type = arg switch
            {
                HealHpModifier => HpModifierTypesModel.Healing,
                DamageHpModifier => HpModifierTypesModel.Damage,
                TemporaryHpModifier => HpModifierTypesModel.Temporary,
                _ => throw new InvalidOperationException($"Unsupported type: {arg.GetType()}")
            },
            Value = arg.Value,
            DamageType = arg switch
            {
                DamageHpModifier d => d.DamageType.ToString(),
                _ => null
            }
        };
    }

    public HpModifier BuildModel()
    {
        switch (Type)
        {
            case "damage":
                return new DamageHpModifier
                {
                    Id = new Id(Id),
                    Value = Value,
                    DamageType = DamageMapper.FromStringName(DamageType??throw new InvalidOperationException("DamageType is null"))
                };
            case "healing":
                return new HealHpModifier
                {
                    Id = new Id(Id),
                    Value = Value
                };
            case "temporary":
                return new TemporaryHpModifier
                {
                    Id = new Id(Id),
                    Value = Value
                };
            default:
                throw new InvalidOperationException($"Unsupported type: {Type}");
        }
    }
}

public class CharacterSheetDbModel
{
    public required string Name { get; init; }
    public int Level { get; init; }
    public int HitPoints { get; init; }
    public required HpModifierDbModel[] HpModifiers { get; init; }
    public required CharacterClass[] Classes { get; init; }
    public required StatsModel Stats { get; init; }
    public Item[]? Items { get; init; }
    public Defence[]? Defenses { get; init; }

    public static CharacterSheetDbModel BuildFromEntity(CharacterSheet arg)
    {
        return new CharacterSheetDbModel
        {
            Name = arg.Name,
            Level = arg.Level,
            HitPoints = arg.HitPoints.Max,
            HpModifiers = arg.HitPoints.HpModifiers.Select(HpModifierDbModel.FromDomainEntity).ToArray(),
            Classes = arg.Classes.ToArray(),
            Stats = new StatsModel
            {
                Strength = arg.Stats.Strength,
                Dexterity = arg.Stats.Dexterity,
                Constitution = arg.Stats.Constitution,
                Intelligence = arg.Stats.Intelligence,
                Wisdom = arg.Stats.Wisdom,
                Charisma = arg.Stats.Charisma
            },
            Items = arg.Items.ToArray(),
            Defenses = arg.Defenses.ToArray()
        };
    }

    public CharacterSheet BuildModel(Id idOfEntity)
    {
        var cs = new CharacterSheet(HitPoints)
        {
            Id = idOfEntity,
            Name = Name,
            Level = Level,
            Classes = Classes.ToList(),
            Stats = new Stats
            {
                Strength = Stats.Strength,
                Dexterity = Stats.Dexterity,
                Constitution = Stats.Constitution,
                Intelligence = Stats.Intelligence,
                Wisdom = Stats.Wisdom,
                Charisma = Stats.Charisma
            },
            Items = Items?.ToList()??[],
            Defenses = Defenses?.ToList()??[]
        };
        foreach (var hpModifierDbModel in HpModifiers) cs.HitPoints.AddHpModifier(hpModifierDbModel.BuildModel());
        return cs;
    }
}