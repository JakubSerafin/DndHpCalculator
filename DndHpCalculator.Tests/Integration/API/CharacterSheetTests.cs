using System.Net;
using System.Text;
using DND_HP_API.CharacterSheet;
using DndHpCalculator.Tests.Integration.API.Helpers;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace DndHpCalculator.Tests.Integration.API;

public class CharacterSheetApiTests
{
    private readonly HttpClient _client;
    private readonly string _characterSheetJson = File.ReadAllText("Data/briv.json");
    private const string CharacterSheetEndpoint = "/CharacterSheet";

    public CharacterSheetApiTests(ITestOutputHelper testOutputHelper)
    {
        var factory = new CustomWebApplicationFactor();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task POST_ShouldUploadAndValidateCharacterSheet()
    {
        var characterSheet = JsonConvert.DeserializeObject<CharacterSheetModel>(_characterSheetJson);
        // POST new character sheet
        var postResponse = await _client.PostAsync(CharacterSheetEndpoint, HttpHelpers.Encode(_characterSheetJson));
        postResponse.Should().BeSuccessful();

        // GET should return the character sheet
        var getResponse = await _client.GetAsync(CharacterSheetEndpoint);
        postResponse.Should().BeSuccessful();

        var responseObject = JsonConvert.DeserializeObject<List<CharacterSheetModel>>(await _responseContent(getResponse));
        responseObject.Should().HaveCount(1)
            .And.ContainEquivalentOf(characterSheet, options => options.Excluding(x => x.Id));
    }
    
    
    [Fact]
    public async void GET_NoCharacterSheetShouldReturnEmptyList()
    {
        var response = await _client.GetAsync(CharacterSheetEndpoint);
        response.Should().BeSuccessful();
        HttpAssertions.AssertResponseContent(response, "[]");
    }
    
    [Fact]
    public async void POST_InvalidCharacterSheetShouldReturnBadRequest()
    {
        var content = new StringContent("{}", Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(CharacterSheetEndpoint,content);
        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async void GET_WithExistingId_ShouldReturnCharacterSheet()
    {
        //POST new character sheet
        var postResponse = await _client.PostAsync(CharacterSheetEndpoint, HttpHelpers.Encode(_characterSheetJson));
        
        //Get should return the character sheet
        var getResponse = await _client.GetAsync(CharacterSheetEndpoint + "/1");
        getResponse.Should().BeSuccessful();
        HttpAssertions.AssertResponseContent(getResponse, _characterSheetJson);
    }
    
    [Fact]
    public async void GET_WithNonExistingId_ShouldReturnNotFound()
    {
        var getResponse = await _client.GetAsync(CharacterSheetEndpoint + "/1");
        getResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    

    
    // a little syntax sugar
    private static async Task<string> _responseContent(HttpResponseMessage response) => await response.Content.ReadAsStringAsync();
    
    
}