using DND_HP_API.Domain;

namespace DND_HP_API.Controllers.ApiModels;

public class ItemModel
{
    public required string Name { get; init; }
    public ModifierModel? Modifier { get; init; }

    internal static ItemModel FromDomainEntity(Item arg)
    {
        return new ItemModel
        {
            Name = arg.Name,
            Modifier = arg.ModifierModel!=null? ModifierModel.FromDomainEntity(arg.ModifierModel):null
        };
    }
}