namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Skills;
using GameLogic.Registry;
using Xunit;

public class RegistryTest
{
    [Fact]
    public void Registry_CanLoadSkills()
    {
        var registry = new Registry();
        registry.Load(
            new List<string>
            {
                ConfigPaths.SkillTemplate.Ignite,
                ConfigPaths.SkillTemplate.FacePalm,
            }
        );

        Assert.True(
            registry.TryGetValue<SkillInstanceSpec>(new ReferenceUnionMetadata(ETemplateType.Skill, EReferenceKind.Ref), out var skill)
        );
        Assert.True(registry.TryGetValue(ConfigPaths.SkillTemplate.FacePalm, out var skill));
    }
}
