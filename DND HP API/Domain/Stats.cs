﻿using DND_HP_API.Domain.Abstract;

namespace DND_HP_API.Domain;

public record Stats:ValueObject
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }
}