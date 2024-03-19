using System.Net;
using System.Text;
using DND_HP_API.Controllers.ApiModels;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DndHpCalculator.Tests.Integration.API.Helpers;

public static class HttpHelpers
{
    public static StringContent Encode(string content)
    {
        return new StringContent(content, Encoding.UTF8, "application/json");
    }
}
public static class HttpAssertions
{
   
    public static async Task AssertResponseContent(HttpResponseMessage response, string expectedContent)
    {
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().Be(expectedContent);
    }

    public static async Task AssertResponseJsonContent<T>(HttpResponseMessage response, T expectedContent,
        Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>>? config = null)
    {
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<T>(responseString);
        responseObject.Should().BeEquivalentTo(expectedContent, config?? (x => x));
    }
}


public class HttpResponseProxy<T>(HttpResponseMessage response)
{
    public async Task<string> RawContent()
    {
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<T?> Content()
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content);
    }

    public HttpStatusCode StatusCode()
    {
        return response.StatusCode;
    }
}
public class FixedHttpClientWrapper<T>(HttpClient client, string path)
{
    public async Task<HttpResponseProxy<T>> Get(string additionalPath = "")
    {
        var response = await client.GetAsync(path + additionalPath);
        return new HttpResponseProxy<T>(response);
    }
    
    public async Task<HttpResponseProxy<T>> Post(string content)
    {
        var response = await client.PostAsync(path, HttpHelpers.Encode(content));
        return new HttpResponseProxy<T>(response);
    }
    
    public async Task<HttpResponseProxy<T>> Post(T content)
    {
        var response = await client.PostAsync(path, HttpHelpers.Encode(JsonConvert.SerializeObject(content)));
        return new HttpResponseProxy<T>(response);
    }
    
    public async Task<HttpResponseProxy<T>> Delete(string additionalPath)
    {
        var response = await client.DeleteAsync(path + additionalPath);
        return new HttpResponseProxy<T>(response);
    }
    
    public async Task<HttpResponseProxy<T>> Put(string additionalPath, string content)
    {
        var response = await client.PutAsync(path + additionalPath, HttpHelpers.Encode(content));
        return new HttpResponseProxy<T>(response);
    }
    
    public async Task<HttpResponseProxy<T>> Put(string additionalPath, T content)
    {
        var response = await client.PutAsync(path + additionalPath, HttpHelpers.Encode(JsonConvert.SerializeObject(content)));
        return new HttpResponseProxy<T>(response);
    }
    
}

public static class StandardRequests
{
    public static async Task<CharacterSheetModel> SeedCharacterSheet(HttpClient client)
    {
        var characterSheetJson = File.ReadAllText("Data/briv.json");
        var content = new StringContent(characterSheetJson, Encoding.UTF8, "application/json");
        var postResponse = await client.PostAsync("/CharacterSheet", content);
        postResponse.Should().BeSuccessful();
        return JsonConvert.DeserializeObject<CharacterSheetModel>(characterSheetJson)!;
    }
    
    public static async Task SeedHpModifiers(HttpClient client, HpModifierModel? modifier  = null)
    {
        await SeedCharacterSheet(client);
        modifier ??= new HpModifierModel()
        {
            Value = 5,
            Type = "Damage", 
            Description = "Test"
        };
        var content = new StringContent(JsonConvert.SerializeObject(modifier), Encoding.UTF8, "application/json");
        var postResponse = await client.PostAsync("/CharacterSheet/1/HpModifiers", content);
        postResponse.Should().BeSuccessful();
    }
}