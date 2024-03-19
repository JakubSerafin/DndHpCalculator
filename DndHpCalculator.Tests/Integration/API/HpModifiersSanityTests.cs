using System.Net;
using DND_HP_API.Controllers.ApiModels;
using DndHpCalculator.Tests.Integration.API.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace DndHpCalculator.Tests.Integration.API;

public class HpModifiersTestsBase
{
    protected readonly HttpClient _client;
    protected const string HpModifiersEndpoint = "/CharacterSheet/1/HpModifiers";
    
    public HpModifiersTestsBase(ITestOutputHelper testOutputHelper)
    {
        var factory = new CustomWebApplicationFactor();
        _client = factory.CreateClient();
    }
}

/*Test for HpModifiers. This functionality is used to modify pool of HP of single player, Therefore is depending on
 The player ID. URL Schema should look like: /CharacterSheet/1/HpModifiers. Also, becuse of that dependecy, here
 are test checking if final HP is calculating right */
public class HpModifiersSanityTests(ITestOutputHelper testOutputHelper) : HpModifiersTestsBase(testOutputHelper)
{
    
    [Fact]
    public async void GET_NoCharacters_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync(HpModifiersEndpoint);
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async void GET_NoHpModifiers_ShouldReturnEmptyList()
    {
        await StandardRequests.SeedCharacterSheet(_client);
        
        var response = await _client.GetAsync(HpModifiersEndpoint);
        response.Should().BeSuccessful();
        HttpAssertions.AssertResponseContent(response, "[]");
    }
    

    [Fact]
    public async void POST_ValidHpModifier_ReturnsSuccessAndAssignId()
    {
        await StandardRequests.SeedCharacterSheet(_client);
        HpModifierModel hpModifier = new HpModifierModel()
        {
            Value = 5,
            Type = "Damage", 
            Description = "Test"
        };
        var postResponse = await _client.PostAsync(HpModifiersEndpoint, HttpHelpers.Encode(JsonConvert.SerializeObject(hpModifier)));
        postResponse.Should().BeSuccessful();
        //Check if response contains new modifier id 
        var responseString = await postResponse.Content.ReadAsStringAsync();
        responseString.Should().Be("1");
        
        var getResponse = await _client.GetAsync(HpModifiersEndpoint+"/1");
        getResponse.Should().BeSuccessful();
        await HttpAssertions.AssertResponseJsonContent(getResponse, hpModifier, 
            opt=>opt.Excluding(qm=>qm.Id));
    }
    
    [Fact]
    public async void DELETE_ValidHpModifier_ReturnsSuccessAndDeletesHpModifier()
    {
        //Seed character sheet
        await StandardRequests.SeedCharacterSheet(_client);
        //Add modifier
        await StandardRequests.SeedHpModifiers(_client);
        //Delete modifier
        var deleteResponse = await _client.DeleteAsync(HpModifiersEndpoint + "/1");
        deleteResponse.Should().BeSuccessful();
        //Check if there are no modifiers
        var getResponse = await _client.GetAsync(HpModifiersEndpoint);
        getResponse.Should().BeSuccessful();
        await HttpAssertions.AssertResponseContent(getResponse, "[]");
    }
    
    [Fact]
    public async void PUT_ValidHpModifier_ReturnsSuccessAndUpdatesHpModifier()
    {
        //Arrange
        //Seed character sheet
        await StandardRequests.SeedCharacterSheet(_client);
        //Add modifier
        await StandardRequests.SeedHpModifiers(_client);
        
        //Act
        //Update modifier
        var modifier = new HpModifierModel()
        {
            Value = 20,
            Type = "Damage", 
            Description = "Test"
        };
        var putResponse = await _client.PutAsync(HpModifiersEndpoint + "/1", HttpHelpers.Encode(JsonConvert.SerializeObject(modifier)));
        
        //Assert
        //Check if modifier is updated
        putResponse.Should().BeSuccessful();
        
        var getResponse = await _client.GetAsync(HpModifiersEndpoint+"/1");
        getResponse.Should().BeSuccessful();
        var queriedModifier = JsonConvert.DeserializeObject<HpModifierModel>(await getResponse.Content.ReadAsStringAsync());
        queriedModifier.Should().BeEquivalentTo(modifier, 
            opt => 
                opt.Excluding(qm => qm.Id)
                );

    }
    
    [Fact]
    public async void GET_ValidHpModifier_ReturnsSuccessAndReturnsListHpModifier()
    {
        //Arrange
        //Seed character sheet
        await StandardRequests.SeedCharacterSheet(_client);
        //Add modifier
        var modifierToSeed = new HpModifierModel()
        {
            Value = 5,
            Type = "Damage",
            Description = "Test"
        };
        await StandardRequests.SeedHpModifiers(_client, modifierToSeed);
        
        //Act
        //Get modifiers
        var getResponse = await _client.GetAsync(HpModifiersEndpoint);
        
        //Assert
        //Check if modifiers are returned
        getResponse.Should().BeSuccessful();
        var modifiers = JsonConvert.DeserializeObject<List<HpModifierModel>>(await getResponse.Content.ReadAsStringAsync());
        //should return one modifier in collection
        modifiers.Should().HaveCount(1);
        // and it should be equal to what we seeded
        modifiers![0].Should().BeEquivalentTo(modifierToSeed, 
            opt => 
                opt.Excluding(qm => qm.Id)
        );
    }
    
    [Fact]
    public async void GET_ValidHpModifierById_ReturnsSuccessAndReturnsHpModifierById()
    {
        //Arrange
        //Seed character sheet
        await StandardRequests.SeedCharacterSheet(_client);
        //Add modifier
        var modifierToSeed = new HpModifierModel()
        {
            Value = 5,
            Type = "Damage",
            Description = "Test"
        };
        await StandardRequests.SeedHpModifiers(_client, modifierToSeed);
        
        //Act
        //Get modifier
        var getResponse = await _client.GetAsync(HpModifiersEndpoint+"/1");
        
        //Assert
        //Check if modifier is returned
        getResponse.Should().BeSuccessful();
        var gotModel = JsonConvert.DeserializeObject<HpModifierModel>(await getResponse.Content.ReadAsStringAsync());
        gotModel.Should().BeEquivalentTo(modifierToSeed, 
            opt => 
                opt.Excluding(qm => qm.Id)
        );
        gotModel!.Id.Should().Be(1);
    }
}

// Test for logic  of HpModifiers. Those are focused on logic, not on API itself. 