using DND_HP_API.Domain;

namespace DND_HP_API.Controllers.ApiModels;

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