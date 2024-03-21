using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public record Item : ValueObject
{
    public required string Name { get; set; }
    public Modifier? ModifierModel { get; set; }
}