using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public record Modifier : ValueObject
{
    public string AffectedObject { get; set; } // What the modifier affects (e.g., stats)
    public string AffectedValue { get; set; } // Specific property to change (e.g., constitution)
    public int Value { get; set; } // The amount to change by
}