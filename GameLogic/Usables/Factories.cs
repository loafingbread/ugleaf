namespace GameLogic.Usables;

using GameLogic.Targeting;
using GameLogic.Usables.Effects;

public static class UsableFactory
{
    public static Usable CreateUsableFromRecord(UsableRecord record)
    {
        return new Usable(
            GameLogic.Registry.Ids.Instance(record.InstanceId),
            GameLogic.Registry.Ids.Template(record.TemplateId),
            record.Name,
            record.Description,
            record.Tags,
            TargetingFactory.CreateFromRecord(record.Targeter),
            record.Effects.Select(EffectFactory.CreateEffectFromRecord).ToList()
        );
    }

    public static Usable CreateUsableFromTemplate(UsableTemplate template)
    {
        return new Usable(
            GameLogic.Registry.Ids.Instance(),
            template.TemplateId,
            template.Name,
            template.Description,
            template.Tags,
            template.Targeter,
            template.Effects
        );
    }

    public static UsableTemplate CreateUsableTemplateFromRecord(UsableTemplateRecord record)
    {
        return new UsableTemplate(
            GameLogic.Registry.Ids.Template(record.TemplateId),
            record.Name,
            record.Description,
            record.Tags,
            TargetingFactory.CreateFromRecord(record.Targeter),
            record.Effects.Select(EffectFactory.CreateEffectFromRecord).ToList()
        );
    }
}
