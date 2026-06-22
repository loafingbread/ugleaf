namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Skills;

public class SkillTestFixture
{
    public SkillTemplateData FacePalmRecord { get; }
    public SkillTemplateData IgniteRecord { get; }
    public SkillTemplateData MugRecord { get; }
    public SkillTemplateData SprayAndPrayRecord { get; }
    public SkillTemplateData StealRecord { get; }

    public SkillTestFixture()
    {
        IgniteRecord = JsonConfigLoader.LoadFromFile<SkillTemplateData>(ConfigPaths.Skill.Ignite);
        FacePalmRecord = JsonConfigLoader.LoadFromFile<SkillTemplateData>(ConfigPaths.Skill.FacePalm);
        MugRecord = JsonConfigLoader.LoadFromFile<SkillTemplateData>(ConfigPaths.Skill.Mug);
        SprayAndPrayRecord = JsonConfigLoader.LoadFromFile<SkillTemplateData>(ConfigPaths.Skill.SprayAndPray);
        StealRecord = JsonConfigLoader.LoadFromFile<SkillTemplateData>(ConfigPaths.Skill.Steal);
    }
}
