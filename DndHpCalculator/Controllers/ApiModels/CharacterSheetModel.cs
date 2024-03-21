using DND_HP_API.Domain;
using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Controllers.ApiModels;

public class CharacterSheetModel
{
    public long? Id { get; set; }
    public required string Name { get; set; }
    public int Level { get; set; }
    public uint HitPoints { get; set; }
    public uint CurrentHitPoints { get; set; }
    public required ClassModel[] Classes { get; set; }
    public required StatsModel Stats { get; set; }
    public ItemModel[]? Items { get; set; }
    public DefenseModel[]? Defenses { get; set; }

    internal static CharacterSheetModel BuildFromEntity(CharacterSheet arg)
    {
        return new CharacterSheetModel
        {
            Id = arg.Id.Value,
            Name = arg.Name,
            Level = arg.Level,
            HitPoints = arg.HitPoints.Max,
            CurrentHitPoints = arg.HitPoints.Current,
            Classes = arg.Classes.Select(ClassModel.FromDomainEntity).ToArray(),
            Stats = new StatsModel
            {
                Strength = arg.Stats.Strength,
                Dexterity = arg.Stats.Dexterity,
                Constitution = arg.Stats.Constitution,
                Intelligence = arg.Stats.Intelligence,
                Wisdom = arg.Stats.Wisdom,
                Charisma = arg.Stats.Charisma
            },
            Items = arg.Items.Select(ItemModel.FromDomainEntity).ToArray(),
            Defenses = arg.Defenses.Select(DefenseModel.FromDomainEntity).ToArray()
        };
    }

    internal CharacterSheet ToDomainEntity()
    {
        return new CharacterSheet(HitPoints)
        {
            Id = Id.HasValue ? new Id(Id.Value) : Domain.Abstract.Id.NewTemporaryId(),
            Name = Name,
            Level = Level,
            Classes = Classes.Select(x => new CharacterClass
            {
                Name = x.Name,
                HitDiceValue = x.HitDiceValue,
                ClassLevel = x.ClassLevel
            }).ToList(),
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
                ModifierModel = x.Modifier!= null? new Modifier
                {
                    AffectedObject = x.Modifier.AffectedObject,
                    AffectedValue = x.Modifier.AffectedValue,
                    Value = x.Modifier.Value
                } : null
            }).ToList() ?? [],
            Defenses = Defenses?.Select(x => new Defence
            {
                Type = DamageMapper.MapDamageType(x.DamageType),
                Defense = DamageMapper.MapDefenceType(x.DefenseType)
            }).ToList() ?? []
        };
    }
}