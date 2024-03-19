using DND_HP_API.Controllers.ApiModels;
using DND_HP_API.Domain;
using DND_HP_API.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DND_HP_API.Controllers;

[ApiController]
[Route("[controller]")]
public class CharacterSheetController(ICharacterSheetRepository characterSheetRepository) : Controller
{
    [HttpGet()]
    public ActionResult<ICollection<CharacterSheetModel>> GetCharacterSheet()
    {
        var models = characterSheetRepository.GetAll();
        var viewModels = models.Select(CharacterSheetModel.BuildFromEntity);
        return Ok(viewModels);
    }

    [HttpGet("{id}")]
    public ActionResult<CharacterSheetModel> GetCharacterSheet(int id)
    {
        var character = characterSheetRepository.Get(id);
        if(character == null)
        {
            return NotFound();
        }
        return Ok(CharacterSheetModel.BuildFromEntity(character));
    }

    [HttpPost()]
    public ActionResult PostCharacterSheet(CharacterSheetModel characterSheet)
    {

        var id = characterSheetRepository.Add(characterSheet.ToDomainEntity());
        return Ok(id.Value);
    }
}