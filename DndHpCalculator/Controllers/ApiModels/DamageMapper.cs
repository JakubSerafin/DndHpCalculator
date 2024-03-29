﻿using DND_HP_API.Domain;

namespace DND_HP_API.Controllers.ApiModels;

internal static class DamageMapper
{
    private static readonly Dictionary<string, DamageType> DamageTypesMap = new()
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

    private static readonly Dictionary<DamageType, string> DamageTypeName = new()
    {
        { DamageType.Bludgeoning, "bludgeoning" },
        { DamageType.Piercing, "piercing" },
        { DamageType.Slashing, "slashing" },
        { DamageType.Fire, "fire" },
        { DamageType.Cold, "cold" },
        { DamageType.Acid, "acid" },
        { DamageType.Thunder, "thunder" },
        { DamageType.Lightning, "lightning" },
        { DamageType.Poison, "poison" },
        { DamageType.Radiant, "radiant" },
        { DamageType.Necrotic, "necrotic" },
        { DamageType.Psychic, "psychic" },
        { DamageType.Force, "force" }
    };

    private static readonly Dictionary<string, DefenceType> DefenceTypesMap = new()
    {
        { "immunity", DefenceType.Immunity },
        { "resistance", DefenceType.Resistance }
    };

    public static string ToStingName(this DamageType damageType)
    {
        return DamageTypeName[damageType];
    }

    public static DamageType FromStringName(string damageType)
    {
        return DamageTypesMap.TryGetValue(damageType.ToLowerInvariant(), out var type)
            ? type
            : throw new InvalidOperationException($"Unsupported damage type: {damageType}");
    }

    public static DamageType MapDamageType(string damageType)
    {
        return DamageTypesMap[damageType.ToLowerInvariant()];
    }

    public static DefenceType MapDefenceType(string defenceType)
    {
        return DefenceTypesMap[defenceType.ToLowerInvariant()];
    }
}