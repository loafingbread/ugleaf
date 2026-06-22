namespace GameLogic.Tests.Registry;

using GameLogic.Config;
using GameLogic.Entities.Skills;
using GameLogic.Registry;
using GameLogic.Usables.Effects.Variants;
using Xunit;

public class SkillTemplateTest
{
    private readonly GameRegistry _registry = new();
    private readonly SkillTemplateFactory _factory = new();

    [Fact]
    public void SkillTemplate_LoadRegisterRetrieve()
    {
        SkillTemplateData data = JsonConfigLoader.LoadFromFile<SkillTemplateData>(
            ConfigPaths.Skill.Ignite
        );
        SkillTemplate template = _factory.Create(ReferenceId.From("skill-ignite"), data);
        _registry.Skills.TryAdd(template.Id, template);

        bool found = _registry.Skills.TryGet(template.Id, out SkillTemplate? retrieved);

        Assert.True(found);
        Assert.NotNull(retrieved);
        Assert.Equal("skill_ignite", retrieved!.ToData().Id);
        Assert.Equal(2, retrieved.Usables.Count);
    }

    [Fact]
    public void SkillTemplate_Usables_HaveCorrectEffectTypes()
    {
        SkillTemplateData data = JsonConfigLoader.LoadFromFile<SkillTemplateData>(
            ConfigPaths.Skill.Ignite
        );
        SkillTemplate template = _factory.Create(ReferenceId.From("skill-ignite"), data);

        Assert.IsType<AttackEffectVariant>(template.Usables[0].Effects[0].EffectVariant);
        Assert.IsType<BurnStatusVariant>(template.Usables[0].Effects[1].EffectVariant);
    }

    [Fact]
    public void SkillTemplate_ToData_RoundTrips()
    {
        SkillTemplateData original = JsonConfigLoader.LoadFromFile<SkillTemplateData>(
            ConfigPaths.Skill.Ignite
        );
        SkillTemplate template = _factory.Create(ReferenceId.From("skill-ignite"), original);

        SkillTemplateData roundTripped = template.ToData();

        Assert.Equal(original.Id, roundTripped.Id);
        Assert.Equal(original.Name, roundTripped.Name);
        Assert.Equal(original.Usables.Count, roundTripped.Usables.Count);
    }
}
