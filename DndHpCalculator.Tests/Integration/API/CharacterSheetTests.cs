using DND_HP_API.CharacterSheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace DndHpCalculator.Tests.Integration.API;

public class CharacterSheetApiTests
{

    private readonly CharacterSheetController _endpoint;
    // load content of briv.json
    private readonly string _characterSheetJson = File.ReadAllText("Data/briv.json");
    private readonly CharacterSheetModel _characterSheet;
    public CharacterSheetApiTests()
    {

        _endpoint = new CharacterSheetController();
        _characterSheet = JsonConvert.DeserializeObject<CharacterSheetModel>(_characterSheetJson)!;
    }

    
    [Fact]
    public void POST_ShouldUploadAndValidateCharacterSheet()
    {
        //Get should return nothing, as there is no character sheet yet 
         var response  = _endpoint.GetCharacterSheet();
         HttpAssertions.AssertOkResult<ICollection<string>>(
             response.Result!, 
             Assert.Empty!);
         
        //POST new character sheet
        _endpoint.PostCharacterSheet(_characterSheet);
        
        //Get should return the character sheet
        response = _endpoint.GetCharacterSheet();
        HttpAssertions.AssertOkResult<ICollection<string>>(
            response.Result!, 
            result => Assert.Contains(_characterSheetJson, result!));
    }
    
    [Fact]
    public void GET_NoCharacterSheetShouldReturnEmptyList()
    {
        var response = _endpoint.GetCharacterSheet();
        HttpAssertions.AssertOkResult<ICollection<string>>(
            response.Result!, 
            Assert.Empty!);
    }
    
    [Fact]
    public void POST_InvalidCharacterSheetShouldReturnBadRequest()
    {
        var response = _endpoint.PostCharacterSheet(new CharacterSheetModel());
        Assert.IsType<BadRequestResult>(response);
    }
    
    [Fact]
    public void GET_WithExistingId_ShouldReturnCharacterSheet()
    {
        //POST new character sheet
        _endpoint.PostCharacterSheet(_characterSheet);
        
        //Get should return the character sheet
        var response = _endpoint.GetCharacterSheet(1);
        HttpAssertions.AssertOkResult<ICollection<string>>(
            response.Result!, 
            result => Assert.Contains(_characterSheetJson, result!));
    }
    
    [Fact]
    public void GET_WithNonExistingId_ShouldReturnNotFound()
    {
        var response = _endpoint.GetCharacterSheet(1);
        HttpAssertions.AssertNotFound(response.Result!);
    }
    
    
}