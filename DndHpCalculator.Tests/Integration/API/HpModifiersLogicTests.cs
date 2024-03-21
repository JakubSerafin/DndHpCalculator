using DND_HP_API.Controllers.ApiModels;
using DND_HP_API.Domain;
using DndHpCalculator.Tests.Integration.API.Helpers;
using FluentAssertions;

namespace DndHpCalculator.Tests.Integration.API;

public class HpModifiersLogicTests : HpModifiersTestsBase, IAsyncLifetime
{
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
                        Type = HpModifierTypesModel.Damage,
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
                        Type = HpModifierTypesModel.Damage
                    },
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = HpModifierTypesModel.Damage
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
                        Type = HpModifierTypesModel.Damage
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
                        Type = HpModifierTypesModel.Healing
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
                        Type = HpModifierTypesModel.Damage
                    },
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = HpModifierTypesModel.Healing
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
                        Type = HpModifierTypesModel.Temporary
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
                        Type = HpModifierTypesModel.Temporary
                    },
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = HpModifierTypesModel.Temporary
                    }
                }, 35,
                "Temporary HP modifier, should use bigger one")
        },

        //Damage should be first taken from temp HP that cannot be healed
        new object[]
        {
            new HpModificatorPermutation(
                new[]
                {
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = HpModifierTypesModel.Temporary
                    },
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = HpModifierTypesModel.Damage
                    },
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = HpModifierTypesModel.Healing
                    }
                }, 30,
                "Damage should be first taken from temp HP that cannot be healed")
        },
        // When temp hp is partially used, new temp hp replaces the old one if it is bigger
        new object[]
        {
            new HpModificatorPermutation(
                new[]
                {
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = HpModifierTypesModel.Temporary
                    },
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = HpModifierTypesModel.Damage
                    },
                    new HpModifierModel
                    {
                        Value = 8,
                        Type = HpModifierTypesModel.Temporary
                    }
                }, 33,
                "When temp hp is partially used, new temp hp replaces the old one (if it is bigger)")
        },
        // When temp hp is partially used, new temp hp is ignored if it is still smaller
        new object[]
        {
            new HpModificatorPermutation(
                new[]
                {
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = HpModifierTypesModel.Temporary
                    },
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = HpModifierTypesModel.Damage
                    },
                    new HpModifierModel
                    {
                        Value = 4,
                        Type = HpModifierTypesModel.Temporary
                    }
                }, 30,
                "When temp hp is partially used, new temp hp is ignored if it is still smaller")
        },
        // complex scenario: damage, add temp hp, adamage for part of temp hp, then heal all damage should finish with max Hp + remaining temp hp
        new object[]
        {
            new HpModificatorPermutation(
                new[]
                {
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = HpModifierTypesModel.Damage
                    },
                    new HpModifierModel
                    {
                        Value = 15,
                        Type = HpModifierTypesModel.Temporary
                    },
                    new HpModifierModel
                    {
                        Value = 5,
                        Type = HpModifierTypesModel.Damage
                    },
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = HpModifierTypesModel.Healing
                    }
                }, 35,
                "complex scenario: damage, add temp hp, adamage for part of temp hp, then heal all damage should finish with max Hp + remaining temp hp")
        }
    };

    //Assuming we make test on briv.json, and character has fire immunity and slashing resistance, and 25 hp total
    public static readonly IEnumerable<object[]> ResistanceTestScenarios = new List<object[]>
    {
        new object[]
        {
            new HpModificatorPermutation
            (
                Description: "Damage type that character is not resistant to should take whole damage",
                Modifiers:
                [
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = HpModifierTypesModel.Damage,
                        DamageType = DamageType.Cold.ToStingName()
                    }
                ],
                ExpectedHp: 15
            )
        },
        new object[]
        {
            new HpModificatorPermutation
            (
                Description: "Damage type that character is resistant to should take half damage",
                Modifiers:
                [
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = HpModifierTypesModel.Damage,
                        DamageType = DamageType.Slashing.ToStingName()
                    }
                ],
                ExpectedHp: 20
            )
        },
        new object[]
        {
            new HpModificatorPermutation
            (
                Description: "Damage type that character is immune to should take no damage",
                Modifiers:
                [
                    new HpModifierModel
                    {
                        Value = 10,
                        Type = HpModifierTypesModel.Damage,
                        DamageType = DamageType.Fire.ToStingName()
                    }
                ],
                ExpectedHp: 25
            )
        }
    };

    private readonly FixedHttpClientWrapper<CharacterSheetModel> _characterSheetClient;
    private readonly FixedHttpClientWrapper<HpModifierModel> _hpModifierClient;

    //it will be initialized in the InitializeAsync method, so null! is to suspend the warning
    private CharacterSheetModel _seededCharacter = null!;

    public HpModifiersLogicTests()
    {
        _characterSheetClient = new FixedHttpClientWrapper<CharacterSheetModel>(Client, "/CharacterSheet");
        _hpModifierClient = new FixedHttpClientWrapper<HpModifierModel>(Client, HpModifiersEndpoint);
    }


    public async Task InitializeAsync()
    {
        //all tests here are based on the same character sheet, so we can seed it once for all tests
        _seededCharacter = await StandardRequests.SeedCharacterSheet(Client);
    }

    public Task DisposeAsync()
    {
        //nothing here.
        return Task.CompletedTask;
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
        characterSheet!.CurrentHitPoints.Should().Be(_seededCharacter.HitPoints);
    }

    [Fact]
    public async void CharacterHasDamageModifiers_CurrentHpShouldBeEqualToMaxHpMinusSumOfDamageModifiers()
    {
        //Arrange
        //Seed character sheet - done in constructor
        //Add modifier
        var modifierToSeed = new HpModifierModel
        {
            Value = 5,
            Type = HpModifierTypesModel.Damage,
            Description = "Test"
        };

        //Act
        await _hpModifierClient.Post(modifierToSeed);
        var characterSheet = await (await _characterSheetClient.Get("/1")).Content();

        //Assert
        //Should not be null and have HP equal to max HP minus modifier value
        characterSheet.Should().NotBeNull();
        characterSheet!.CurrentHitPoints.Should().Be(_seededCharacter.HitPoints - modifierToSeed.Value);
    }

    [Fact]
    public async void CharacterLoseDamageModifiers_CurrentHpShouldGrewBack()
    {
        //Arrange
        //Seed character sheet - done in constructor
        //Add modifier
        var modifierToSeed = new HpModifierModel
        {
            Value = 5,
            Type = HpModifierTypesModel.Damage
        };


        //Act
        //Add some damage
        var damagePosted = await _hpModifierClient.Post(modifierToSeed);
        // Check status
        var damageId = await damagePosted.Content();
        var characterSheetAfterDamage = await (await _characterSheetClient.Get("/1")).Content();
        //Remove some damage
        await _hpModifierClient.Delete($"/{damageId}");
        // Check status
        var characterSheetAfterRemovingDamage = await (await _characterSheetClient.Get("/1")).Content();


        //Assert
        //Should have subtracted hp after taking damage
        characterSheetAfterDamage!.CurrentHitPoints.Should()
            .Be(_seededCharacter.HitPoints - modifierToSeed.Value);
        //But should have grown back after removing damage
        characterSheetAfterRemovingDamage!.CurrentHitPoints.Should()
            .Be(_seededCharacter.HitPoints);
    }

    [Theory(DisplayName = "Health modifiers should affect character HP")]
    [MemberData(nameof(HpModifierTypeShouldAffectCharacterHpData))]
    public async void HpModifierTypeShouldAffectCharacterHp(HpModificatorPermutation permuations)
    {
        //Arrange
        //Seed character sheet - done in constructor
        //Act
        //Add modifier
        foreach (var hpModifierModel in permuations.Modifiers) await _hpModifierClient.Post(hpModifierModel);
        var characterSheet = await (await _characterSheetClient.Get("/1")).Content();
        //Assert
        //Should not be null and have HP equal to max HP minus modifier value
        characterSheet.Should().NotBeNull();
        characterSheet!.CurrentHitPoints.Should().Be(permuations.ExpectedHp);
    }

    [Theory(DisplayName = "Resistances should alter damage dealed to the character")]
    [MemberData(nameof(ResistanceTestScenarios))]
    public async void ResistancesShouldAlterDamageDealedToTheCharacter(HpModificatorPermutation permuations)
    {
        //Arrange
        //Seed character sheet - done in constructor
        //Act
        //Add modifier
        foreach (var hpModifierModel in permuations.Modifiers) await _hpModifierClient.Post(hpModifierModel);
        var characterSheet = await (await _characterSheetClient.Get("/1")).Content();
        //Assert
        //Should not be null and have HP equal to max HP minus modifier value
        characterSheet.Should().NotBeNull();
        characterSheet!.CurrentHitPoints.Should().Be(permuations.ExpectedHp);
    }

    public record HpModificatorPermutation(HpModifierModel[] Modifiers, uint ExpectedHp, string Description = "")
    {
        public override string ToString()
        {
            return Description;
        }
    }
}