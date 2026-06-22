namespace GameLogic.Tests.Registry;

using GameLogic.Config;
using GameLogic.Registry;
using GameLogic.Usables;
using GameLogic.Usables.Effects.Variants;
using Xunit;

public class UsableTemplateTest
{
    private readonly GameRegistry _registry = new();
    private readonly UsableTemplateFactory _factory = new();

    [Fact]
    public void UsableTemplate_LoadRegisterRetrieve()
    {
        UsableTemplateData data = JsonConfigLoader.LoadFromFile<UsableTemplateData>(
            ConfigPaths.Usable.IgniteUsable
        );
        UsableTemplate template = _factory.Create(ReferenceId.From("usable-ignite"), data);
        _registry.Usables.TryAdd(template.Id, template);

        bool found = _registry.Usables.TryGet(template.Id, out UsableTemplate? retrieved);

        Assert.True(found);
        Assert.NotNull(retrieved);
        Assert.Equal("usable_ignite", retrieved!.ToData().Id);
        Assert.Equal(2, retrieved.Effects.Count);
    }

    [Fact]
    public void UsableTemplate_Effects_AreCorrectTypes()
    {
        UsableTemplateData data = JsonConfigLoader.LoadFromFile<UsableTemplateData>(
            ConfigPaths.Usable.IgniteUsable
        );
        UsableTemplate template = _factory.Create(ReferenceId.From("usable-ignite"), data);

        Assert.IsType<AttackEffectVariant>(template.Effects[0].EffectVariant);
        Assert.IsType<BurnStatusVariant>(template.Effects[1].EffectVariant);
    }

    [Fact]
    public void UsableTemplate_ToData_RoundTrips()
    {
        UsableTemplateData original = JsonConfigLoader.LoadFromFile<UsableTemplateData>(
            ConfigPaths.Usable.IgniteUsable
        );
        UsableTemplate template = _factory.Create(ReferenceId.From("usable-ignite"), original);

        UsableTemplateData roundTripped = template.ToData();

        Assert.Equal(original.Id, roundTripped.Id);
        Assert.Equal(original.Effects.Count, roundTripped.Effects.Count);
        Assert.Equal(original.Effects[0].Type, roundTripped.Effects[0].Type);
    }
}
