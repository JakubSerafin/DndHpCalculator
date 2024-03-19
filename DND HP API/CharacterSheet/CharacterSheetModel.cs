using System.Text.Json.Serialization;
using DND_HP_API.Domain;

namespace DND_HP_API.CharacterSheet;

public class CharacterSheetModel// In English, "Character" is the equivalent of "Postać"
{ 
    public int? Id { get; set; } // Public property to store the character's ID
    public required string Name { get; set; } // Public property to store the character's name
    public int Level { get; set; } // Public property for the character's level 
    public int HitPoints { get; set; } // Public property for hit points
    public int CurrentHitPoints { get; set; } // CalculatedProperty of the character's current hit points
    public ClassModel[] Classes { get; set; } // Array of "Class" objects for multiple classes
    public StatsModel Stats { get; set; } // Holds the character's statistics
    public ItemModel[]? Items { get; set; } // Array to hold the character's items
    public DefenseModel[]? Defenses { get; set; } // Array of "Defense" objects
    
    internal static CharacterSheetModel BuildFromEntity(Domain.CharacterSheet arg)
    {
        return new CharacterSheetModel
        {
            Id = arg.Id.Value,
            Name = arg.Name,
            Level = arg.Level,
            HitPoints =  arg.HitPoints.Max,
            CurrentHitPoints = arg.HitPoints.Current,
            Classes = arg.Classes.Select(ClassModel.FromDomainEntity).ToArray(),
            Stats =  new StatsModel
            {
                Strength = arg.Stats.Strength,
                Dexterity = arg.Stats.Dexterity,
                Constitution = arg.Stats.Constitution,
                Intelligence = arg.Stats.Intelligence,
                Wisdom = arg.Stats.Wisdom,
                Charisma = arg.Stats.Charisma
            },
            Items = arg.Items?.Select(ItemModel.FromDomainEntity).ToArray()??[],
            Defenses = arg.Defenses?.Select(DefenseModel.FromDomainEntity).ToArray()??[]
        };
    }

    internal Domain.CharacterSheet ToDomainEntity()
    {
        return new Domain.CharacterSheet
        {
            //TODO: It should be resolved somehow diffrent, maybe repository should be responsible for creating new Ids based on DTO? 
            Id = Id.HasValue ? new Id(Id.Value) : Domain.Id.NewTemporaryId(),
            Name = Name,
            Level = Level,
            HitPoints = new HitPoints(HitPoints),
            Classes = Classes.Select(x => new CharacterClass
            {
                Name = x.Name,
                HitDiceValue = x.HitDiceValue,
                ClassLevel = x.ClassLevel
            }).ToArray(),
            Stats = new Stats
            {
                Strength = Stats.Strength,
                Dexterity = Stats.Dexterity,
                Constitution = Stats.Constitution,
                Intelligence = Stats.Intelligence,
                Wisdom = Stats.Wisdom,
                Charisma = Stats.Charisma
            },
            Items = Items?.Select(x => new Item
            {
                Name = x.Name,
                ModifierModel = new Modifier
                {
                    AffectedObject = x.Modifier.AffectedObject,
                    AffectedValue = x.Modifier.AffectedValue,
                    Value = x.Modifier.Value
                }
            }).ToArray(),
            Defenses = Defenses?.Select(x => new Defence
            {
                Type = DamageMapper.MapDamageType(x.DamageType),
                Defense = DamageMapper.MapDefenceType(x.DefenseType)
            }).ToArray()
        };
    }
}

public class ClassModel 
{
    public string Name { get; set; } 
    public int HitDiceValue { get; set; }
    public int ClassLevel { get; set; }

    internal static ClassModel FromDomainEntity(CharacterClass arg)
    {
        return new ClassModel
        {
            Name = arg.Name,
            HitDiceValue = arg.HitDiceValue,
            ClassLevel = arg.ClassLevel
        };
    }
}

public class StatsModel
{
    public int Strength { get; set; } 
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }
}

public class ItemModel 
{
    public string Name { get; set; }
    public ModifierModel Modifier { get; set; } 
    
    internal static ItemModel FromDomainEntity(Item arg)
    {
        return new ItemModel
        {
            Name = arg.Name,
            Modifier = ModifierModel.FromDomainEntity(arg.ModifierModel)
        };
    }
}

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

public class DefenseModel
{
    [JsonPropertyName("type")]
    public string DamageType { get; set; } // Type of damage (fire, slashing, etc.)
    [JsonPropertyName("defense")]
    public string DefenseType { get; set; } // Kind of defense (immunity, resistance, etc.)

    internal static DefenseModel FromDomainEntity(Defence arg)
    {
        return new DefenseModel
        {
            DamageType = arg.Type.ToString(),
            DefenseType = arg.Defense.ToString()
        };
    }
}
