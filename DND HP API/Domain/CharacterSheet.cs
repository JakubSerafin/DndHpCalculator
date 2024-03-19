namespace DND_HP_API.Domain;

public record Id(int Value, bool IsTemporary = false)
{
    public int Value { get; set; } = Value;
    public bool IsTemporary { get; set; } = IsTemporary;

    public static Id NewTemporaryId(int value=-1)
    {
        return new Id(value, true);
    }
}

public abstract class Entity
{
    public Id Id { get; set; }
}

public abstract record ValueObject
{
}

public record HitPoints(int Max)
{
    public int Max { get; set; } = Max;

    public int Current
    {
        get
        {
            var step = new HpModifier.HpModifierInnerCalculationStep(Max, 0, Max);
            foreach (var hpModifier in _hpModifiers)
            {
                step = hpModifier.ModifyLifePool(step);
            }
            return step.Current + step.Temp;
        }
    }
    
    private readonly List<HpModifier> _hpModifiers = [];

    public IReadOnlyList<HpModifier> HpModifiers => _hpModifiers;

    public void AddHpModifier(HpModifier hpModifier)
    {
        // Assign an Id to the HpModifier
        hpModifier.Id = Id.NewTemporaryId(_hpModifiers.Count + 1);
        _hpModifiers.Add(hpModifier);
    }

    public bool RemoveHpModifier(int id)
    {
        var deletedCount = _hpModifiers.RemoveAll(m => m.Id.Value == id);
        return deletedCount > 0;
    }
}

public class CharacterSheet: Entity
{

    public required string Name { get; set; } // Public property to store the character's name
    public int Level { get; set; } // Public property for the character's level 
    public HitPoints HitPoints { get; set; } // Public property for hit points
    

    public CharacterClass[] Classes { get; set; }
    public Stats Stats { get; set; }
    
    public Item[]? Items { get; set; }
    public Defence[]? Defenses { get; set; }
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

internal static class DamageMapper
{
    private static readonly Dictionary<string, DamageType> DamageTypesMap = new Dictionary<string, DamageType>
    {
        { "bludgeoning", DamageType.Bludgeoning },
        { "piercing", DamageType.Piercing },
        { "slashing", DamageType.Slashing },
        { "fire", DamageType.Fire },
        { "cold", DamageType.Cold },
        { "acid", DamageType.Acid },
        { "thunder", DamageType.Thunder },
        { "lightning", DamageType.Lightning },
        { "poison", DamageType.Poison },
        { "radiant", DamageType.Radiant },
        { "necrotic", DamageType.Necrotic },
        { "psychic", DamageType.Psychic },
        { "force", DamageType.Force }
    };

    private static readonly Dictionary<string, DefenceType> DefenceTypesMap = new Dictionary<string, DefenceType>
    {
        { "immunity", DefenceType.Immunity },
        { "resistance", DefenceType.Resistance }
    };

    public static DamageType MapDamageType(string damageType)
    {
        return DamageTypesMap[damageType.ToLowerInvariant()];
    }

    public static DefenceType MapDefenceType(string defenceType)
    {
        return DefenceTypesMap[defenceType.ToLowerInvariant()];
    }
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


public abstract class HpModifier: Entity
{
    public record HpModifierInnerCalculationStep(int Current, int Temp, int Max);
    public int Value { get; set; }

    public abstract HpModifierInnerCalculationStep ModifyLifePool(HpModifierInnerCalculationStep step);
}
    
public class HealHpModifier: HpModifier
{
    public override HpModifierInnerCalculationStep ModifyLifePool(HpModifierInnerCalculationStep step)
    {
        var calculated = step with {Current = Math.Min(step.Current + Value, step.Max)};
        return calculated;
    }
}

public class DamageHpModifier: HpModifier
{
    public override HpModifierInnerCalculationStep ModifyLifePool(HpModifierInnerCalculationStep step)
    {
        var remainingDamage = Math.Max(0, Value - step.Temp);
        return step with 
        {
            Temp = Math.Max(0, step.Temp - Value),
            Current = Math.Max(0, step.Current - remainingDamage)
        };
    }
}

public class TemporaryHpModifier: HpModifier
{
    public override HpModifierInnerCalculationStep ModifyLifePool(HpModifierInnerCalculationStep step)
    {
        var calculated = step with {Temp = Math.Max(step.Temp, Value)};
        return calculated;
    }
}
