using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public record Modifier : ValueObject
{
    public required string AffectedObject { get; init; } // What the modifier affects (e.g., stats)
    public required string AffectedValue { get; init; } // Specific property to change (e.g., constitution)
    public int Value { get; init; } // The amount to change by
}