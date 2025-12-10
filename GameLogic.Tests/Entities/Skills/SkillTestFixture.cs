namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Skills;
using GameLogic.Registry;

public class SkillTestFixture
{
    public ReferenceSpec FacePalmRecord { get; }
    public ReferenceSpec IgniteRecord { get; }
    public ReferenceSpec MugRecord { get; }
    public ReferenceSpec SprayAndPrayRecord { get; }
    public ReferenceSpec StealRecord { get; }

    public SkillTestFixture()
    {
        IgniteRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.SkillTemplate.Ignite
        );
        FacePalmRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.SkillTemplate.FacePalm
        );
        MugRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.SkillTemplate.Mug
        );
        SprayAndPrayRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.SkillTemplate.SprayAndPray
        );
        StealRecord = JsonConfigLoader.LoadFromFile<ReferenceSpec>(
            ConfigPaths.SkillTemplate.Steal
        );
    }
}
