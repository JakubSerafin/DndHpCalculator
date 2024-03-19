using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public record Defence: ValueObject
{
    public DamageType Type { get; set; }
    public DefenceType Defense { get; set; }
    
}