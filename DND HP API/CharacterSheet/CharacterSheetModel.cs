namespace DND_HP_API.CharacterSheet;

public class CharacterSheetModel // In English, "Character" is the equivalent of "Postać"
{
    public string Name { get; set; } // Public property to store the character's name
    public int Level { get; set; } // Public property for the character's level 
    public int HitPoints { get; set; } // Public property for hit points
    public Class[] Classes { get; set; } // Array of "Class" objects for multiple classes
    public Stats Stats { get; set; } // Holds the character's statistics
    public Item[] Items { get; set; } // Array to hold the character's items
    public Defense[] Defenses { get; set; } // Array of "Defense" objects
}

public class Class 
{
    public string Name { get; set; } 
    public int HitDiceValue { get; set; }
    public int ClassLevel { get; set; } 
}

public class Stats
{
    public int Strength { get; set; } 
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }
}

public class Item 
{
    public string Name { get; set; }
    public Modifier Modifier { get; set; } 
}

public class Modifier
{
    public string AffectedObject { get; set; } // What the modifier affects (e.g., stats)
    public string AffectedValue { get; set; } // Specific property to change (e.g., constitution)
    public int Value { get; set; } // The amount to change by
}

public class Defense 
{
    public string Type { get; set; } // Type of damage (fire, slashing, etc.)
    public string DefenseType { get; set; } // Kind of defense (immunity, resistance, etc.)
}
