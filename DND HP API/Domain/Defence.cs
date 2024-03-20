using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public record Defence: ValueObject
{
    public DamageType Type { get; set; }
    public DefenceType Defense { get; set; }

    public int Reduce(DamageType damageType, int damage)
    {
        //reducing damage based on defence type. Rounded down
        return damageType == Type ? (int)Math.Floor(damage*(1-Defense.ReduceFactor()))  : damage;
    }
}