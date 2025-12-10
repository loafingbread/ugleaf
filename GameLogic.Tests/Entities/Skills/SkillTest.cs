namespace GameLogic.Tests;

using System.Text.Json;
using GameLogic.Config;
using GameLogic.Entities.Skills;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;
using GameLogic.Usables.Effects;
using Xunit;
using Xunit.Abstractions;

public class SkillTest : IClassFixture<SkillTestFixture>
{
    private readonly SkillTestFixture _fixture;
    private readonly ITestOutputHelper _output;

    public SkillTest(SkillTestFixture fixture, ITestOutputHelper output)
    {
        this._fixture = fixture;
        this._output = output;
    }

    [Fact]
    public void Skill_CanLoadFromFile()
    {
        // Print FacePalmRecord as JSON
        JsonSerializerOptions jsonOptions = new(JsonConfigLoader.options)
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
        };
        string facePalmJson = JsonSerializer.Serialize(this._fixture.FacePalmRecord, jsonOptions);
        // string igniteJson = JsonSerializer.Serialize(this._fixture.IgniteRecord, jsonOptions);
        // string mugJson = JsonSerializer.Serialize(this._fixture.MugRecord, jsonOptions);
        // string sprayAndPrayJson = JsonSerializer.Serialize(this._fixture.SprayAndPrayRecord, jsonOptions);
        // string stealJson = JsonSerializer.Serialize(this._fixture.StealRecord, jsonOptions);

        Console.WriteLine($"facePalmJson: {facePalmJson}");
        // Console.WriteLine($"igniteJson: {igniteJson}");
        // Console.WriteLine($"mugJson: {mugJson}");
        // Console.WriteLine($"sprayAndPrayJson: {sprayAndPrayJson}");
        // Console.WriteLine($"stealJson: {stealJson}");

        // this._output.WriteLine("FacePalmRecord JSON:");
        // this._output.WriteLine(facePalmJson);

        Reference<SkillTemplate, Skill> facePalmReference =
            SkillFactory.CreateSkillInstanceSpecFromRecord(this._fixture.FacePalmRecord);

        if (facePalmReference.Template is null)
        {
            throw new InvalidOperationException("Face Palm template is null");
        }

        SkillTemplate facePalm = facePalmReference.Template;

        Console.WriteLine($"facePalm: ");
        Console.WriteLine(facePalm.ReferenceMetadata.TemplateId.ToString());
        Console.WriteLine(facePalm.Name);
        Console.WriteLine(facePalm.Targeter?.Count);
        Console.WriteLine(facePalm.Targeter?.QuantityType);
        Console.WriteLine(facePalm.Targeter?.AllowedTargets.ToString());
        Console.WriteLine(facePalm.Usables.Count);
        Console.WriteLine(facePalm.Usables[0].Targeter?.Count);
        Console.WriteLine(facePalm.Usables[0].Targeter?.QuantityType);
        Console.WriteLine(facePalm.Usables[0].Targeter?.AllowedTargets.ToString());
        Console.WriteLine(facePalm.Usables[0].Effects.Count);


        Effect? effect = facePalm.Usables[0].Effects[0] as Effect;
        Console.WriteLine(effect?.Type);
        Console.WriteLine(effect?.Subtype);
        Console.WriteLine(effect?.Variant);
        Console.WriteLine(effect?.Name);
        Console.WriteLine(effect?.Description);
        Console.WriteLine(effect?.Tags.ToString());
        Console.WriteLine(effect?.Value);
        Console.WriteLine(effect?.Duration);


        Assert.Equal("skill_facepalm", facePalm.ReferenceMetadata.TemplateId.ToString());
        Assert.Equal("Face Palm", facePalm.Name);
        Assert.Equal(1, facePalm.Targeter?.Count);
        Assert.Equal(ETargetQuantity.Count, facePalm.Targeter?.QuantityType);
        Assert.Equal([EFactionRelationship.Self], facePalm.Targeter?.AllowedTargets);
    }

    // [Fact]
    // public void Skill_CanLoadFullFromFile()
    // {
    //     Reference<SkillTemplate, Skill> igniteReference =
    //         SkillFactory.CreateSkillInstanceSpecFromRecord(this._fixture.IgniteRecord);

    //     if (igniteReference.Template is null)
    //     {
    //         throw new InvalidOperationException("Ignite template is null");
    //     }

    //     SkillTemplate ignite = igniteReference.Template;

    //     Assert.Equal("skill_ignite", ignite.ReferenceMetadata.TemplateId.ToString());
    //     Assert.Equal("Ignite", ignite.Name);

    //     Usable? igniteUsable = ignite.Usables[0] as Usable;
    //     Assert.NotNull(igniteUsable);

    //     Targeter? igniteUsableTargeter = igniteUsable.Targeter as Targeter;
    //     Assert.NotNull(igniteUsableTargeter);
    //     Assert.Equal(ETargetQuantity.Count, igniteUsableTargeter.QuantityType);
    //     Assert.Equal([EFactionRelationship.Enemy], igniteUsableTargeter.AllowedTargets);
    //     Assert.Equal(1, igniteUsableTargeter.Count);

    //     IEffect secondEffect = igniteUsable.Effects[1];

    //     BurnStatusEffect? burnEffect = secondEffect as BurnStatusEffect;
    //     Assert.NotNull(burnEffect);
    //     Assert.Equal("effect_burn_dot", burnEffect.ReferenceMetadata.TemplateId.ToString());
    //     Assert.Equal(EEffectType.Status, burnEffect.Type);
    //     Assert.Equal("Burn", burnEffect.Subtype);
    //     Assert.Equal("DOT", burnEffect.Variant);
    //     Assert.Equal(3, burnEffect.Duration);
    //     Assert.Equal(5, burnEffect.Value);
    // }
}
