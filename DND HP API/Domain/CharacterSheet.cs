using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public class CharacterSheet: Entity
{

    public required string Name { get; set; } // Public property to store the character's name
    public int Level { get; set; } // Public property for the character's level 
    public HitPoints HitPoints { get; set; } // Public property for hit points
    

    public CharacterClass[] Classes { get; set; }
    public Stats Stats { get; set; }
    
    public Item[]? Items { get; set; }
    public Defence[]? Defenses { get; set; }
}