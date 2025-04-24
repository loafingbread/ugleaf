namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Skills;

public class SkillTestFixture
{
    public SkillConfig FacePalmConfig { get; }
    public SkillConfig MugConfig { get; }
    public SkillConfig SprayAndPrayConfig { get; }
    public SkillConfig StealConfig { get; }

    public SkillTestFixture()
    {
        FacePalmConfig = JsonConfigLoader.LoadFromFile<SkillConfig>(ConfigPaths.Skill.FacePalm);
        MugConfig = JsonConfigLoader.LoadFromFile<SkillConfig>(ConfigPaths.Skill.Mug);
        SprayAndPrayConfig = JsonConfigLoader.LoadFromFile<SkillConfig>(
            ConfigPaths.Skill.SprayAndPray
        );
        StealConfig = JsonConfigLoader.LoadFromFile<SkillConfig>(ConfigPaths.Skill.Steal);
    }
}
