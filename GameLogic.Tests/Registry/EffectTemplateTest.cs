namespace GameLogic.Tests.Registry;

using GameLogic.Config;
using GameLogic.Registry;
using GameLogic.Usables.Effects;
using GameLogic.Usables.Effects.Variants;
using Xunit;

public class EffectTemplateTest
{
    private readonly GameRegistry _registry = new();
    private readonly EffectTemplateFactory _factory = new();

    [Fact]
    public void AttackEffect_LoadRegisterRetrieve()
    {
        EffectTemplateData data = JsonConfigLoader.LoadFromFile<EffectTemplateData>(
            ConfigPaths.Effect.AttackEffect
        );
        EffectTemplate template = _factory.Create(ReferenceId.From("attack-effect"), data);
        _registry.Effects.TryAdd(template.Id, template);

        bool found = _registry.Effects.TryGet(template.Id, out EffectTemplate? retrieved);

        Assert.True(found);
        Assert.NotNull(retrieved);
        Assert.Equal("Attack", retrieved!.Type);
        Assert.Equal("SingleHit", retrieved.Subtype);
        Assert.IsType<AttackEffectVariant>(retrieved.EffectVariant);
    }

    [Fact]
    public void BurnEffect_LoadRegisterRetrieve()
    {
        EffectTemplateData data = JsonConfigLoader.LoadFromFile<EffectTemplateData>(
            ConfigPaths.Effect.BurnStatus
        );
        EffectTemplate template = _factory.Create(ReferenceId.From("burn-status"), data);
        _registry.Effects.TryAdd(template.Id, template);

        bool found = _registry.Effects.TryGet(template.Id, out EffectTemplate? retrieved);

        Assert.True(found);
        Assert.NotNull(retrieved);
        Assert.Equal("Status", retrieved!.Type);
        Assert.Equal("Burn", retrieved.Subtype);
        Assert.IsType<BurnStatusVariant>(retrieved.EffectVariant);
    }

    [Fact]
    public void EffectTemplate_ToData_RoundTrips()
    {
        EffectTemplateData original = JsonConfigLoader.LoadFromFile<EffectTemplateData>(
            ConfigPaths.Effect.AttackEffect
        );
        EffectTemplate template = _factory.Create(ReferenceId.From("attack-effect"), original);

        EffectTemplateData roundTripped = template.ToData();

        Assert.Equal(original.Type, roundTripped.Type);
        Assert.Equal(original.Subtype, roundTripped.Subtype);
        Assert.Equal(original.Value, roundTripped.Value);
        Assert.Equal(original.CritChance, roundTripped.CritChance);
    }

    [Fact]
    public void EffectTemplatePatch_ApplyTo_OverridesFields()
    {
        EffectTemplateData baseData = new()
        {
            Type = "Attack",
            Subtype = "SingleHit",
            Value = 10f,
            CritChance = 0.1f,
        };
        EffectTemplatePatch patch = new() { Value = 20f };

        EffectTemplateData patched = patch.ApplyTo(baseData);

        Assert.Equal(20f, patched.Value);
        Assert.Equal(0.1f, patched.CritChance);
        Assert.Equal("Attack", patched.Type);
    }
}
