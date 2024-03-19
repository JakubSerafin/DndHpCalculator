using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public class CharacterClass: Entity
{
    public string Name { get; set; }
    public int HitDiceValue { get; set; }
    public int ClassLevel { get; set; }
}