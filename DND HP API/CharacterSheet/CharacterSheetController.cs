

using Microsoft.AspNetCore.Mvc;

namespace DND_HP_API.CharacterSheet;

[ApiController]
[Route("[controller]")]
public class CharacterSheetController : Controller
{
    private readonly ICharacterSheetRepository _characterSheetRepository;

    public CharacterSheetController(ICharacterSheetRepository characterSheetRepository)
    {
        this._characterSheetRepository = characterSheetRepository;
    }
    
    [HttpGet()]
    public ActionResult<ICollection<CharacterSheetModel>> GetCharacterSheet()
    {
        var models = _characterSheetRepository.GetAll();
        var viewModels = models.Select(CharacterSheetModel.BuildFromEntity);
        return Ok();
    }

    [HttpGet("{id}")]
    public ActionResult<CharacterSheetModel> GetCharacterSheet(int id)
    {
        var character = _characterSheetRepository.Get(id);
        if(character == null)
        {
            return NotFound();
        }
        return Ok(CharacterSheetModel.BuildFromEntity(character));
    }

    [HttpPost()]
    public ActionResult PostCharacterSheet(CharacterSheetModel characterSheet)
    {

        _characterSheetRepository.Add(characterSheet.ToDomainEntity());
        return Ok();
    }
}