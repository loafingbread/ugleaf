using GameLogic.Entities.Skills;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public static class SkillFactory
{
    public static Skill CreateSkillFromRecord(ReferenceSpec record)
    {
        return new Skill(
            record.Metadata.InstanceId,
            record.Metadata,
            record.TemplateIdentifier,
            record.Name,
            record.Description,
            record.Tags,
            TargetingFactory.CreateFromRecord(record.Targeter),
            record.Usables.Select(UsableFactory.CreateUsableFromRecord).ToList()
        );
    }

    public static SkillTemplate CreateSkillTemplateFromRecord(SkillTemplateSpec record)
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
