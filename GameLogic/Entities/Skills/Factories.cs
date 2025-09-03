using GameLogic.Entities.Skills;
using GameLogic.Targeting;
using GameLogic.Usables;

public static class SkillFactory
{
    public static Skill CreateSkillFromRecord(SkillRecord record)
    {
        return new Skill(
            GameLogic.Registry.Ids.Instance(record.InstanceId),
            GameLogic.Registry.Ids.Template(record.TemplateId),
            record.Name,
            record.Description,
            record.Tags,
            TargetingFactory.CreateFromRecord(record.Targeter),
            record.Usables.Select(UsableFactory.CreateUsableFromRecord).ToList()
        );
    }

    public static Skill CreateSkillFromTemplate(SkillTemplate template)
    {
        return new Skill(
            GameLogic.Registry.Ids.Instance(),
            template.TemplateId,
            template.Name,
            template.Description,
            template.Tags,
            template.Targeter,
            template.Usables
        );
    }

    public static SkillTemplate CreateSkillTemplateFromRecord(SkillTemplateRecord record)
    {
        return new SkillTemplate(
            new GameLogic.Registry.TemplateId(record.TemplateId),
            record.Name,
            record.Description,
            record.Tags,
            TargetingFactory.CreateFromRecord(record.Targeter),
            record.Usables.Select(UsableFactory.CreateUsableFromRecord).ToList()
        );
    }
}
