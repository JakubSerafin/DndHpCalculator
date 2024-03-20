using System.Text.Json.Serialization;
using DND_HP_API.Domain;

namespace DND_HP_API.Controllers.ApiModels;

public class DefenseModel
{
    [JsonPropertyName("type")] public required string DamageType { get; init; } // Type of damage (fire, slashing, etc.)

    [JsonPropertyName("defense")]
    public required string DefenseType { get; init; } // Kind of defense (immunity, resistance, etc.)

    internal static DefenseModel FromDomainEntity(Defence arg)
    {
        return new DefenseModel
        {
            DamageType = arg.Type.ToString(),
            DefenseType = arg.Defense.ToString()
        };
    }
}