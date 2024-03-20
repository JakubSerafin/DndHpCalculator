using DND_HP_API.Domain;

namespace DND_HP_API.Controllers.ApiModels;

public class ItemModel
{
    public string Name { get; set; }
    public ModifierModel Modifier { get; set; }

    internal static ItemModel FromDomainEntity(Item arg)
    {
        return new ItemModel
        {
            Name = arg.Name,
            Modifier = ModifierModel.FromDomainEntity(arg.ModifierModel)
        };
    }
}