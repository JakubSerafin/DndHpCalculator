using DND_HP_API.CharacterSheet;

namespace DND_HP_API.Domain;

public abstract class Entity
{
    public int Id { get; set; }
}

public abstract record ValueObject
{
}

public class CharacterSheet: Entity
{
    public required string Name { get; set; } // Public property to store the character's name
    public int Level { get; set; } // Public property for the character's level 
    public int HitPoints { get; set; } // Public property for hit points

    public int CurrentHitPoints
    {
        get
        {
            return HitPoints - _hpModifiers.Sum(x => x.Value);
        }
    }

    public CharacterClass[] Classes { get; set; }
    public Stats Stats { get; set; }
    
    public Item[]? Items { get; set; }
    public Defence[]? Defenses { get; set; }

    private List<HpModifiers> _hpModifiers = new();
}

public class CharacterClass: Entity
{
    public string Name { get; set; }
    public int HitDiceValue { get; set; }
    public int ClassLevel { get; set; }
}

public record Stats:ValueObject
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }
}

public record Item: ValueObject
{
    public string Name { get; set; }
    public Modifier ModifierModel { get; set; } 
}

public record Modifier : ValueObject
{
    public string AffectedObject { get; set; } // What the modifier affects (e.g., stats)
    public string AffectedValue { get; set; } // Specific property to change (e.g., constitution)
    public int Value { get; set; } // The amount to change by
}

public record Defence: ValueObject
{
    public DamageType Type { get; set; }
    public DefenceType Defense { get; set; }
}

public enum DamageType
{
    Bludgeoning,
    Piercing,
    Slashing,
    Fire,
    Cold,
    Acid,
    Thunder,
    Lightning,
    Poison,
    Radiant,
    Necrotic,
    Psychic,
    Force
}

public enum DefenceType
{
    Immunity,
    Resistance
}




public class HpModifiers: Entity
{
    public int Value { get; set; }
}