

using Microsoft.AspNetCore.Mvc;

namespace DND_HP_API.CharacterSheet;

[ApiController]
[Route("[controller]")]
public class CharacterSheetController: Controller
{
    private readonly List<CharacterSheetModel> _characterSheets = [];
    
    [HttpGet()]
    public ActionResult<ICollection<CharacterSheetModel>> GetCharacterSheet()
    {
        return Ok(_characterSheets);
    }
    
    [HttpGet("{id}")]
    public ActionResult<CharacterSheetModel> GetCharacterSheet(int id)
    {
        if (id < 0 || id >= _characterSheets.Count)
        {
            return NotFound();
        }
        return Ok(_characterSheets[id]);
    }
    [HttpPost()]
    public ActionResult PostCharacterSheet(CharacterSheetModel characterSheet)
    {
        _characterSheets.Add(characterSheet);
        return Ok();
    }
    
}