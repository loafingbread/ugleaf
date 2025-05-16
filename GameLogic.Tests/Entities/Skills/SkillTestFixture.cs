namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Skills;

public class SkillTestFixture
{
    public SkillRecord FacePalmRecord { get; }
    public SkillRecord IgniteRecord { get; }
    public SkillRecord MugRecord { get; }
    public SkillRecord SprayAndPrayRecord { get; }
    public SkillRecord StealRecord { get; }

    public SkillTestFixture()
    {
        IgniteRecord = JsonConfigLoader.LoadFromFile<SkillRecord>(ConfigPaths.Skill.Ignite);
        FacePalmRecord = JsonConfigLoader.LoadFromFile<SkillRecord>(ConfigPaths.Skill.FacePalm);
        MugRecord = JsonConfigLoader.LoadFromFile<SkillRecord>(ConfigPaths.Skill.Mug);
        SprayAndPrayRecord = JsonConfigLoader.LoadFromFile<SkillRecord>(
            ConfigPaths.Skill.SprayAndPray
        );
        StealRecord = JsonConfigLoader.LoadFromFile<SkillRecord>(ConfigPaths.Skill.Steal);
    }
}
