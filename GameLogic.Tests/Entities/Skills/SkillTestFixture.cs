namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Skills;
using GameLogic.Registry;

public class SkillTestFixture
{
    public ReferenceUnionSpec FacePalmRecord { get; }
    public ReferenceUnionSpec IgniteRecord { get; }
    public ReferenceUnionSpec MugRecord { get; }
    public ReferenceUnionSpec SprayAndPrayRecord { get; }
    public ReferenceUnionSpec StealRecord { get; }

    public SkillTestFixture()
    {
        IgniteRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.SkillTemplate.Ignite
        );
        FacePalmRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.SkillTemplate.FacePalm
        );
        MugRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.SkillTemplate.Mug
        );
        SprayAndPrayRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.SkillTemplate.SprayAndPray
        );
        StealRecord = JsonConfigLoader.LoadFromFile<ReferenceUnionSpec>(
            ConfigPaths.SkillTemplate.Steal
        );
    }
}
