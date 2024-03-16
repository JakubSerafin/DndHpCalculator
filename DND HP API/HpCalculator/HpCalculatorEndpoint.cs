using System.Collections;
using DND_HP_API.CharacterSheet;
using Microsoft.AspNetCore.Mvc;
namespace DND_HP_API.HpCalculator;


// public API for this module 
[ApiController]
[Route("CharacterSheet/{characterId:int}/[controller]")]
public class HpModifiersController
    : ControllerBase
{
    private readonly ICharacterSheetRepository _characterSheetRepository;
    private readonly IHpModifierRepository _hpModifierRepository;

    public HpModifiersController(
        ICharacterSheetRepository characterSheetRepository,
        IHpModifierRepository hpModifierRepository)
    {
        this._characterSheetRepository = characterSheetRepository;
        this._hpModifierRepository = hpModifierRepository;
    }

    [HttpGet()]
    public IActionResult GetModifiers([FromRoute] int characterId)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if(characterSheet == null)
        {
            return NotFound();
        }
        return Ok(_hpModifierRepository.GetAll());
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetModifierById([FromRoute] int characterId, [FromRoute] int id)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if(characterSheet == null)
        {
            return NotFound();
        }

        var modifier = _hpModifierRepository.Get(id);
        if(modifier == null)
        {
            return NotFound();
        }
        return Ok(modifier);
    }

    [HttpPost]
    public IActionResult AddModifier([FromRoute] int characterId, [FromBody] HpModifierModel hpModifier)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if(characterSheet == null)
        {
            return NotFound();
        }
        _hpModifierRepository.Add(hpModifier);
        return Ok(hpModifier.Id);
    }

    [HttpDelete("{id:int}")]
    public IActionResult RemoveModifier([FromRoute] int id, [FromRoute] int characterId)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if(characterSheet == null)
        {
            return NotFound();
        }
        var wasDeleted = _hpModifierRepository.Delete(id);
        
        if(wasDeleted)
        {
            return Ok();
        }
        return NoContent();
    }
    
    [HttpPut("{id:int}")]
    public IActionResult UpdateModifier([FromRoute] int id, [FromRoute] int characterId, [FromBody] HpModifierModel hpModifier)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if(characterSheet == null)
        {
            return NotFound();
        }

        var existingMod = _hpModifierRepository.Get(id);
        if(existingMod == null)
        {
            return NotFound();
        }
        //TODO - this is risky, we should update it in the repository
        existingMod.Value = hpModifier.Value;
        existingMod.Type = hpModifier.Type;
        existingMod.Description = hpModifier.Description;
        return Ok();
    }
    
    private Domain.CharacterSheet? GetCharacterSheet(int characterId)
    {
        return _characterSheetRepository.Get(characterId);
    }
}