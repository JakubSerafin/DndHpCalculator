using DND_HP_API.Controllers.ApiModels;
using DND_HP_API.Domain;
using DND_HP_API.Domain.Abstract;
using DND_HP_API.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DND_HP_API.Controllers;

// public API for this module 
[ApiController]
[Authorize(Roles = "GameMaster")]
[Route("CharacterSheet/{characterId:int}/[controller]")]
public class HpModifiersController(
    ICharacterSheetRepository characterSheetRepository,
    IHpModifierRepository hpModifierRepository)
    : ControllerBase
{
    [HttpGet]
    public IActionResult GetModifiers([FromRoute] int characterId)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if (characterSheet == null) return NotFound();
        var modifiers = characterSheet.HitPoints.HpModifiers.Select(HpModifierModel.FromEntity);
        return Ok(modifiers);
    }

    [HttpGet("{id:long}")]
    public IActionResult GetModifierById([FromRoute] int characterId, [FromRoute] long id)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if (characterSheet == null) return NotFound();

        var modifier = characterSheet.HitPoints.HpModifiers.FirstOrDefault(m => m.Id.Value == id);
        if (modifier == null) return NotFound();
        return Ok(HpModifierModel.FromEntity(modifier));
    }

    [HttpPost]
    public IActionResult AddModifier([FromRoute] int characterId, [FromBody] HpModifierModel hpModifier)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if (characterSheet == null) return NotFound();
        var hpModifierEntity = hpModifier.BuildEntity();
        characterSheet.HitPoints.AddHpModifier(hpModifierEntity);
        characterSheetRepository.Add(characterSheet);
        return Ok(hpModifierEntity.Id.Value.ToString());
    }

    [HttpDelete("{id}")]
    public IActionResult RemoveModifier([FromRoute] string id, [FromRoute] int characterId)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if (characterSheet == null) return NotFound();
        var wasDeleted = characterSheet.HitPoints.RemoveHpModifier(Id.NewTemporaryId(id));
        characterSheetRepository.Add(characterSheet);

        if (wasDeleted) return Ok();
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateModifier([FromRoute] string id, [FromRoute] int characterId,
        [FromBody] HpModifierModel hpModifier)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if (characterSheet == null) return NotFound();

        var mod = characterSheet.HitPoints.HpModifiers.FirstOrDefault(modifier => modifier.Id.Equals(id));
        if (mod == null) return NotFound();

        characterSheet.HitPoints.ReplaceModifier(mod.Id, hpModifier.BuildEntity());
        //
        characterSheetRepository.Add(characterSheet);
        return Ok();
    }

    private CharacterSheet? GetCharacterSheet(int characterId)
    {
        return characterSheetRepository.Get(characterId);
    }
}