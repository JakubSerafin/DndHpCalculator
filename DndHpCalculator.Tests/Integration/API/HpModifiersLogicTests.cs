using DND_HP_API.CharacterSheet;
using DND_HP_API.HpCalculator;
using DndHpCalculator.Tests.Integration.API.Helpers;
using FluentAssertions;
using Xunit.Abstractions;

namespace DndHpCalculator.Tests.Integration.API;

public class HpModifiersLogicTests: HpModifiersTestsBase, IAsyncLifetime
{
    private readonly FixedHttpClientWrapper<CharacterSheetModel> _characterSheetClient;
    private readonly FixedHttpClientWrapper<HpModifierModel> _hpModifierClient;
    public HpModifiersLogicTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
        _characterSheetClient = new FixedHttpClientWrapper<CharacterSheetModel>(_client, "/CharacterSheet");
        _hpModifierClient = new FixedHttpClientWrapper<HpModifierModel>(_client, HpModifiersEndpoint);
    }

    [Fact]
    public async void CharacterHasNoModifiers_CurrentHpShouldBeEqualToMaxHp()
    {
        //Arrange 
        //Nothing here, because we want to test the default state of the character sheet
        //Act
        var characterSheet = await (await _characterSheetClient.Get("/1")).Content();
        //Assert
        //Should not be null and have HP equal to max HP
        characterSheet.Should().NotBeNull();
        characterSheet!.CurrentHitPoints.Should().Be(characterSheet.HitPoints);
    }
    
    [Fact]
    public async void CharacterHasDamageModifiers_CurrentHpShouldBeEqualToMaxHpMinusSumOfDamageModifiers()
    {
        //Arrange
        //Seed character sheet - done in constructor
        //Add modifier
        var modifierToSeed = new HpModifierModel()
        {
            Value = 5,
            Type = "Damage",
            Description = "Test"
        };
        
        //Act
        await _hpModifierClient.Post(modifierToSeed);
        var characterSheet = await (await _characterSheetClient.Get("/1")).Content();
        
        //Assert
        //Should not be null and have HP equal to max HP minus modifier value
        characterSheet.Should().NotBeNull();
        characterSheet!.CurrentHitPoints.Should().Be(characterSheet.HitPoints - modifierToSeed.Value);
    }
    
    [Fact]
    public async void CharacterLoseDamageModifiers_CurrentHpShouldGrewBack()
    {
        //Arrange
        //Seed character sheet - done in constructor
        //Add modifier
        var modifierToSeed = new HpModifierModel()
        {
            Value = 5,
        };
        
        
        //Act
        //Add some damage
        var dagamePosted = await _hpModifierClient.Post(modifierToSeed);
        // Check status
        var characterSheetAfterDamage = await (await _characterSheetClient.Get("/1")).Content();
        //Remove some damage
        await _hpModifierClient.Delete("/1");
        // Check status
        var characterSheetAfterRemovingDamage = await (await _characterSheetClient.Get("/1")).Content();
        
        
        //Assert
        //Should have subtracted hp after taking damage
        characterSheetAfterDamage!.CurrentHitPoints.Should()
            .Be(characterSheetAfterDamage.HitPoints - modifierToSeed.Value);
        //But should have grown back after removing damage
        characterSheetAfterRemovingDamage!.CurrentHitPoints.Should()
            .Be(characterSheetAfterDamage.HitPoints);
    }

    public record HpModificatorPermutation(HpModifierModel[] Modifiers, int ExpectedHp, string Description = "")
    {
        public override string ToString()
        {
            return Description;
        }
    };
    public static readonly IEnumerable<object[]> HpModifierTypeShouldAffectCharacterHpData = new List<object[]>
    {
        new object[]
        {
            new HpModificatorPermutation(
                new[]
                {
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = "Damage",
                        Description = "Test"
                    }
                }, 20, "Single Damage modifier")
        },
        new object[]
        {
            new HpModificatorPermutation(
                new[]
                {
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = "Damage",
                    },
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = "Damage",
                    }
                }, 10, "Multiple Damage modifiers")
        },
        new object[]
        {
            new HpModificatorPermutation(
                new[]
                {
                    new HpModifierModel
                    {
                        Value = 100,
                        Type = "Damage",
                    }
                }, 0, 
                "Health should never go below 0")
        },
        new object[]
        {
            new HpModificatorPermutation(
                new[]
                {
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = "Heal",
                    }
                }, 25, 
                "Single Heal modifier, should not go above max HP")
        },
        new object[]
        {
            new HpModificatorPermutation(
                new[]
                {
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = "Damage",
                    },
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = "Heal",
                    }
                }, 20, 
                "Damage then heal modifier")
        },
        //Give a character temporary HP
        new object[]
        {
            new HpModificatorPermutation(
                new[]
                {
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = "Temporary",
                    }
                }, 30, 
                "Temporary HP modifier")
        },
        //Give character two temporary HP modifiers - only bigger one should be used
        new object[]
        {
            new HpModificatorPermutation(
                new[]
                {
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = "Temporary",
                    },
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = "Temporary",
                    }
                }, 35, 
                "Temporary HP modifier, should use bigger one")
        },
    };
    
    [Theory(DisplayName = "Health modifiers should affect character HP")]
    [MemberData(nameof(HpModifierTypeShouldAffectCharacterHpData))]
    public async void HpModifierTypeShouldAffectCharacterHp(HpModificatorPermutation permuations)
    {
        //Arrange
        //Seed character sheet - done in constructor
        //Act
        //Add modifier
        foreach (var hpModifierModel in permuations.Modifiers)
        {
            await _hpModifierClient.Post(hpModifierModel);
        }
        var characterSheet = await (await _characterSheetClient.Get("/1")).Content();
        //Assert
        //Should not be null and have HP equal to max HP minus modifier value
        characterSheet.Should().NotBeNull();
        characterSheet!.CurrentHitPoints.Should().Be(permuations.ExpectedHp);
    }

    public async Task InitializeAsync()
    {
        //all tests here are based on the same character sheet, so we can seed it once for all tests
        await StandardRequests.SeedCharacterSheet(_client);
    }

    public Task DisposeAsync()
    {
        //nothing here.
        return Task.CompletedTask;
    }
}