using GameLogic.Entities.Skills;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public static class SkillFactory
{
    public static Skill CreateSkillFromRecord(ReferenceUnionSpec record)
    {
        return new Skill(
            GameLogic.Registry.Ids.Instance(record.InstanceId),
            record.TemplateIdentifier,
            record.Name,
            record.Description,
            record.Tags,
            TargetingFactory.CreateFromRecord(record.Targeter),
            record.Usables.Select(UsableFactory.CreateUsableFromRecord).ToList()
        );
    }

    public static Skill

    public static Skill CreateSkillFromTemplate(SkillTemplate template)
    {
        return new Skill(
            GameLogic.Registry.Ids.Instance(),
            template.TemplateIdentifier,
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
            record.TemplateIdentifier,
            record.Name,
            record.Description,
            record.Tags,
            TargetingFactory.CreateFromRecord(record.Targeter),
            record.Usables.Select(UsableFactory.CreateUsableFromRecord).ToList()
        );
    }
}
