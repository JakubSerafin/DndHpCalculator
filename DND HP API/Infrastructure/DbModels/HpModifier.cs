using DND_HP_API.Controllers.ApiModels;
using DND_HP_API.Domain;
using DND_HP_API.Domain.Abstract;
using DND_HP_API.Domain.HpModifiers;

namespace DnDHpCalculator.Database.DbModels;

public class HpModifierDbModel
{
    public long Id { get; set; }
    public int Value { get; set; }
    public string Type { get; set; }
    
    public string? DamageType { get; set; }
    public string Description { get; set; }

    public static HpModifierDbModel FromDomainEntity(HpModifier arg)
    {
        return new HpModifierDbModel
        {
            Description = "temp",
            Id = !arg.Id.IsTemporary?arg.Id.Value:DateTime.Now.Ticks,
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
            case "Damage":
                return new DamageHpModifier
                {
                    Id = new Id(Id),
                    Value = Value,
                    DamageType = DamageMapper.FromStringName(DamageType)
                };
            case "Healing":
                return new HealHpModifier
                {
                    Id = new Id(Id),
                    Value = Value,
                };
            case "Temporary":
                return new TemporaryHpModifier
                {
                    Id = new Id(Id),
                    Value = Value,
                };
            default:
                throw new InvalidOperationException($"Unsupported type: {Type}");
        }
    }
}

public class CharacterSheetDbModel
{
    public required string Name { get; set; }
    public int Level { get; set; }  
    public int HitPoints { get; set; } 
    public HpModifierDbModel[] HpModifiers { get; set; } 
    public CharacterClass[] Classes { get; set; } 
    public StatsModel Stats { get; set; } 
    public Item[]? Items { get; set; } 
    public Defence[]? Defenses { get; set; }
    
    public static CharacterSheetDbModel BuildFromEntity(CharacterSheet arg)
    {
        return new CharacterSheetDbModel
        {
            Name = arg.Name,
            Level = arg.Level,
            HitPoints =  arg.HitPoints.Max,
            HpModifiers = arg.HitPoints.HpModifiers.Select(HpModifierDbModel.FromDomainEntity).ToArray(),
            Classes = arg.Classes,
            Stats =  new StatsModel
            {
                Strength = arg.Stats.Strength,
                Dexterity = arg.Stats.Dexterity,
                Constitution = arg.Stats.Constitution,
                Intelligence = arg.Stats.Intelligence,
                Wisdom = arg.Stats.Wisdom,
                Charisma = arg.Stats.Charisma
            },
            Items = arg.Items,
            Defenses = arg.Defenses
        };
    }
    
    public CharacterSheet BuildModel()
    {
        var cs =  new CharacterSheet(HitPoints)
        {
            Name = this.Name,
            Level = this.Level,
            Classes = this.Classes,
            Stats = new Stats
            {
                Strength = this.Stats.Strength,
                Dexterity = this.Stats.Dexterity,
                Constitution = this.Stats.Constitution,
                Intelligence = this.Stats.Intelligence,
                Wisdom = this.Stats.Wisdom,
                Charisma = this.Stats.Charisma
            },
            Items = this.Items??[],
            Defenses = this.Defenses??[]
        };
        foreach (var hpModifierDbModel in this.HpModifiers)
        {
            cs.HitPoints.AddHpModifier(hpModifierDbModel.BuildModel());
        }
        return cs;
    }
}
