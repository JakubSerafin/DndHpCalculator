using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public class CharacterSheet : Entity
{
    public CharacterSheet(uint maxHp)
    {
        HitPoints = new HitPoints(maxHp, this);
    }
    public required string Name { get; set; }
    public int Level { get; set; }
    public HitPoints HitPoints { get; }
    public List<CharacterClass> Classes { get; init; } = [];
    public required Stats Stats { get; init; }
    public List<Item> Items { get; init; } = [];
    public List<Defence> Defenses { get; init; } = [];
}