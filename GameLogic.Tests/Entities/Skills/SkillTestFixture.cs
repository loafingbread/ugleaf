namespace GameLogic.Tests;

using GameLogic.Config;
using GameLogic.Entities.Skills;

public class SkillTestFixture
{
    public SkillTemplateRecord FacePalmRecord { get; }
    public SkillTemplateRecord IgniteRecord { get; }
    public SkillTemplateRecord MugRecord { get; }
    public SkillTemplateRecord SprayAndPrayRecord { get; }
    public SkillTemplateRecord StealRecord { get; }

    public SkillTestFixture()
    {
        IgniteRecord = JsonConfigLoader.LoadFromFile<SkillTemplateRecord>(
            ConfigPaths.SkillTemplate.Ignite
        );
        FacePalmRecord = JsonConfigLoader.LoadFromFile<SkillTemplateRecord>(
            ConfigPaths.SkillTemplate.FacePalm
        );
        MugRecord = JsonConfigLoader.LoadFromFile<SkillTemplateRecord>(
            ConfigPaths.SkillTemplate.Mug
        );
        SprayAndPrayRecord = JsonConfigLoader.LoadFromFile<SkillTemplateRecord>(
            ConfigPaths.SkillTemplate.SprayAndPray
        );
        StealRecord = JsonConfigLoader.LoadFromFile<SkillTemplateRecord>(
            ConfigPaths.SkillTemplate.Steal
        );
    }
}
