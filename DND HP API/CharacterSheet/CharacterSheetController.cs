using Microsoft.AspNetCore.Mvc;

namespace DND_HP_API.CharacterSheet;

public class CharacterSheetController: ControllerBase
{
    private List<string> _characterSheets = [];
    [HttpGet]
    public ActionResult<ICollection<string>> GetCharacterSheet()
    {
        return Ok(_characterSheets);
    }
    
    [HttpGet("{id}")]
    public ActionResult<string> GetCharacterSheet(int id)
    {
        if (id < 0 || id >= _characterSheets.Count)
        {
            return NotFound();
        }
        return Ok(_characterSheets[id]);
    }
    [HttpPost]
    public ActionResult PostCharacterSheet(string characterSheet)
    {
        
        _characterSheets.Add(characterSheet);
        return Ok();
    }
    
}