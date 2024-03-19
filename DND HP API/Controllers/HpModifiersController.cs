﻿using DND_HP_API.Controllers.ApiModels;
using DND_HP_API.Domain;
using DND_HP_API.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DND_HP_API.Controllers;


// public API for this module 
[ApiController]
[Route("CharacterSheet/{characterId:int}/[controller]")]
public class HpModifiersController(
    ICharacterSheetRepository characterSheetRepository,
    IHpModifierRepository hpModifierRepository)
    : ControllerBase
{
    [HttpGet()]
    public IActionResult GetModifiers([FromRoute] int characterId)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if(characterSheet == null)
        {
            return NotFound();
        }
        return Ok(characterSheet.HitPoints.HpModifiers.Select(HpModifierModel.FromEntity));
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetModifierById([FromRoute] int characterId, [FromRoute] int id)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if(characterSheet == null)
        {
            return NotFound();
        }

        var modifier = characterSheet.HitPoints.HpModifiers.FirstOrDefault(m => m.Id.Value == id);
        if(modifier == null)
        {
            return NotFound();
        }
        return Ok(HpModifierModel.FromEntity(modifier));
    }

    [HttpPost]
    public IActionResult AddModifier([FromRoute] int characterId, [FromBody] HpModifierModel hpModifier)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if(characterSheet == null)
        {
            return NotFound();
        }
        var hpModifierEntity = hpModifier.BuildEntity();
        characterSheet.HitPoints.AddHpModifier(hpModifierEntity);
        characterSheetRepository.Add(characterSheet);
        return Ok(hpModifierEntity.Id.Value);
    }

    [HttpDelete("{id:int}")]
    public IActionResult RemoveModifier([FromRoute] int id, [FromRoute] int characterId)
    {
        var characterSheet = GetCharacterSheet(characterId);
        if(characterSheet == null)
        {
            return NotFound();
        }
        var wasDeleted = characterSheet.HitPoints.RemoveHpModifier(id);
        characterSheetRepository.Add(characterSheet);
        
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

        var existingMod = hpModifierRepository.Get(id);
        if(existingMod == null)
        {
            return NotFound();
        }
        //TODO - this is risky, we should update it in the repository
        existingMod.Value = hpModifier.Value;
        return Ok();
    }
    
    private Domain.CharacterSheet? GetCharacterSheet(int characterId)
    {
        return characterSheetRepository.Get(characterId);
    }
}