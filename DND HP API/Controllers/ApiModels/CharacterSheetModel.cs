using DND_HP_API.Domain;
using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Controllers.ApiModels;

public class CharacterSheetModel// In English, "Character" is the equivalent of "Postać"
{ 
    public long? Id { get; set; } // Public property to store the character's ID
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
            Id = Id.HasValue ? new Id(Id.Value) : Domain.Abstract.Id.NewTemporaryId(),
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