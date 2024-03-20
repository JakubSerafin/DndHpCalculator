using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public class CharacterSheet: Entity
{
    public CharacterSheet(int maxHp)
    {
        HitPoints = new HitPoints(maxHp,this);
    }
    public required string Name { get; set; }
    public int Level { get; set; }
    public HitPoints HitPoints { get; }
    

    public CharacterClass[] Classes { get; set; }
    public Stats Stats { get; set; }
    
    public Item[]? Items { get; set; }
    public Defence[]? Defenses { get; set; } 
}