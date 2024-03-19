using DND_HP_API.Domain;

namespace DND_HP_API.Controllers.ApiModels;

public class ClassModel 
{
    public string Name { get; set; } 
    public int HitDiceValue { get; set; }
    public int ClassLevel { get; set; }

    internal static ClassModel FromDomainEntity(CharacterClass arg)
    {
        return new ClassModel
        {
            Name = arg.Name,
            HitDiceValue = arg.HitDiceValue,
            ClassLevel = arg.ClassLevel
        };
    }
}