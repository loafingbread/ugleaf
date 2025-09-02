using GameLogic.Entities.Skills;

public static class SkillFactory
{
    public static Skill CreateSkillFromRecord(SkillRecord record)
    {
        return new Skill(
            new GameLogic.Registry.InstanceId(record.InstanceId),
            new GameLogic.Registry.TemplateId(record.TemplateId),
            record.Name,
            record.Description,
            record.Tags,
            record.Targeter,
            record.Usables
        );
    }

    public static Skill CreateSkillFromTemplate(SkillTemplate template)
    {
        return new Skill(
            GameLogic.Registry.Ids.Instance(),
            template.Id,
            template.Metadata.DisplayName,
            template.Metadata.Description,
            template.Metadata.Tags,
            template.Config.Targeter,
            template.Config.Usables
        );
    }

    public static SkillTemplate CreateSkillTemplateFromRecord(SkillTemplateRecord record)
    {
        return new SkillTemplate(record);
    }
}
