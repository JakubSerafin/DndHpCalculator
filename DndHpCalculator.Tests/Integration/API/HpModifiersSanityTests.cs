using System.Net;
using DND_HP_API.Controllers.ApiModels;
using DndHpCalculator.Tests.Integration.API.Helpers;
using FluentAssertions;
using Newtonsoft.Json;

namespace DndHpCalculator.Tests.Integration.API;

public class HpModifiersTestsBase
{
    protected const string HpModifiersEndpoint = "/CharacterSheet/1/HpModifiers";
    protected readonly HttpClient Client;

    public HpModifiersTestsBase()
    {
        var factory = new CustomWebApplicationFactor();
        Client = factory.CreateClient();
        Client.DefaultRequestHeaders.Add("x-api-key", "your-api-key");
    }
}

/*Test for HpModifiers. This functionality is used to modify pool of HP of single player, Therefore is depending on
 The player ID. URL Schema should look like: /CharacterSheet/1/HpModifiers. Also, becuse of that dependecy, here
 are test checking if final HP is calculating right */
public class HpModifiersSanityTests : HpModifiersTestsBase
{
    [Fact]
    public async void GET_NoCharacters_ShouldReturnNotFound()
    {
        var response = await Client.GetAsync(HpModifiersEndpoint);
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void GET_NoHpModifiers_ShouldReturnEmptyList()
    {
        await StandardRequests.SeedCharacterSheet(Client);

        var response = await Client.GetAsync(HpModifiersEndpoint);
        response.Should().BeSuccessful();
        await HttpAssertions.AssertResponseContent(response, "[]");
    }


    [Fact]
    public async void POST_ValidHpModifier_ReturnsSuccessAndAssignId()
    {
        await StandardRequests.SeedCharacterSheet(Client);
        var hpModifier = new HpModifierModel
        {
            Value = 5,
            Type = HpModifierTypesModel.Damage,
            Description = "Test"
        };
        var postResponse = await Client.PostAsync(HpModifiersEndpoint,
            HttpHelpers.Encode(JsonConvert.SerializeObject(hpModifier)));
        postResponse.Should().BeSuccessful();
        //Check if response contains new modifier id 
        var responseString = await postResponse.Content.ReadAsStringAsync();
        responseString.Should().NotBeNullOrEmpty();

        var getResponse = await Client.GetAsync(HpModifiersEndpoint + $"/{responseString}");
        getResponse.Should().BeSuccessful();
        await HttpAssertions.AssertResponseJsonContent(getResponse, hpModifier,
            opt => opt.Excluding(qm => qm.Id));
    }

    [Fact]
    public async void DELETE_ValidHpModifier_ReturnsSuccessAndDeletesHpModifier()
    {
        //Seed character sheet
        await StandardRequests.SeedCharacterSheet(Client);
        //Add modifier
        var id = await StandardRequests.SeedHpModifiers(Client);
        //Delete modifier
        var deleteResponse = await Client.DeleteAsync(HpModifiersEndpoint + $"/{id}");
        deleteResponse.Should().BeSuccessful();
        //Check if there are no modifiers
        var getResponse = await Client.GetAsync(HpModifiersEndpoint);
        getResponse.Should().BeSuccessful();
        await HttpAssertions.AssertResponseContent(getResponse, "[]");
    }

    [Fact]
    public async void PUT_ValidHpModifier_ReturnsSuccessAndUpdatesHpModifier()
    {
        //Arrange
        //Seed character sheet
        await StandardRequests.SeedCharacterSheet(Client);
        //Add modifier
        var id = await StandardRequests.SeedHpModifiers(Client);

        //Act
        //Update modifier
        var modifier = new HpModifierModel
        {
            Value = 20,
            Type = HpModifierTypesModel.Damage,
            Description = "Test"
        };
        var putResponse = await Client.PutAsync(HpModifiersEndpoint + $"/{id}",
            HttpHelpers.Encode(JsonConvert.SerializeObject(modifier)));

        //Assert
        //Check if modifier is updated
        putResponse.Should().BeSuccessful();


        var getResponse = await Client.GetAsync(HpModifiersEndpoint + $"/{id}");
        getResponse.Should().BeSuccessful();
        var queriedModifier =
            JsonConvert.DeserializeObject<HpModifierModel>(await getResponse.Content.ReadAsStringAsync());
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
        await StandardRequests.SeedCharacterSheet(Client);
        //Add modifier
        var modifierToSeed = new HpModifierModel
        {
            Value = 5,
            Type = HpModifierTypesModel.Damage,
            Description = "Test"
        };
        await StandardRequests.SeedHpModifiers(Client, modifierToSeed);

        //Act
        //Get modifiers
        var getResponse = await Client.GetAsync(HpModifiersEndpoint);

        //Assert
        //Check if modifiers are returned
        getResponse.Should().BeSuccessful();
        var modifiers =
            JsonConvert.DeserializeObject<List<HpModifierModel>>(await getResponse.Content.ReadAsStringAsync());
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
        await StandardRequests.SeedCharacterSheet(Client);
        //Add modifier
        var modifierToSeed = new HpModifierModel
        {
            Value = 5,
            Type = HpModifierTypesModel.Damage,
            Description = "Test"
        };
        var id = await StandardRequests.SeedHpModifiers(Client, modifierToSeed);

        //Act
        //Get modifier
        var getResponse = await Client.GetAsync(HpModifiersEndpoint + $"/{id}");

        //Assert
        //Check if modifier is returned
        getResponse.Should().BeSuccessful();
        var gotModel = JsonConvert.DeserializeObject<HpModifierModel>(await getResponse.Content.ReadAsStringAsync());
        gotModel.Should().BeEquivalentTo(modifierToSeed,
            opt =>
                opt.Excluding(qm => qm.Id)
        );
        gotModel!.Id.Should().Be(id);
    }
}

// Test for logic  of HpModifiers. Those are focused on logic, not on API itself. 