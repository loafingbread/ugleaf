namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Skills;

public class SkillTestFixture
{
    public SkillRecord FacePalmConfig { get; }
    public SkillRecord IgniteConfig { get; }
    public SkillRecord MugConfig { get; }
    public SkillRecord SprayAndPrayConfig { get; }
    public SkillRecord StealConfig { get; }

    public SkillTestFixture()
    {
        IgniteConfig = JsonConfigLoader.LoadFromFile<SkillRecord>(ConfigPaths.Skill.Ignite);
        FacePalmConfig = JsonConfigLoader.LoadFromFile<SkillRecord>(ConfigPaths.Skill.FacePalm);
        MugConfig = JsonConfigLoader.LoadFromFile<SkillRecord>(ConfigPaths.Skill.Mug);
        SprayAndPrayConfig = JsonConfigLoader.LoadFromFile<SkillRecord>(
            ConfigPaths.Skill.SprayAndPray
        );
        StealConfig = JsonConfigLoader.LoadFromFile<SkillRecord>(ConfigPaths.Skill.Steal);
    }
}
