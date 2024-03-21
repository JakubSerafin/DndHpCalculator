using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public record CharacterClass : ValueObject
{
    public required string Name { get; init; }
    public int HitDiceValue { get; init; }
    public int ClassLevel { get; init; }
}