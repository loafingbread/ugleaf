namespace GameLogic.Entities.Characters;

using System.Linq;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Utils;

public static class CharacterFactory
{
    public static Character CreateCharacterFromRecord(CharacterRecord record)
    {
        return new Character(
            GameLogic.Registry.Ids.Instance(record.InstanceId),
            GameLogic.Registry.Ids.Template(record.TemplateId),
            record.Name,
            record.Description,
            record.Tags,
            StatFactory.CreateStatBlockFromRecord(record),
            record
                .Skills.Select((SkillRecord skill) => SkillFactory.CreateSkillFromRecord(skill))
                .ToList()
        );
    }

    public static Character CreateCharacterFromTemplate(CharacterTemplate template)
    {
        return new Character(
            GameLogic.Registry.Ids.Instance(),
            template.TemplateId,
            template.Name,
            template.Description,
            template.Tags,
            template.Stats.DeepCopy(),
            template.Skills.DeepCopyList()
        );
    }

    public static CharacterTemplate CreateCharacterTemplateFromRecord(
        CharacterTemplateRecord record
    )
    {
        return new CharacterTemplate(
            GameLogic.Registry.Ids.Template(record.TemplateId),
            record.Name,
            record.Description,
            record.Tags,
            StatFactory.CreateStatBlockFromRecord(record),
            record.Skills.Select(SkillFactory.CreateSkillFromRecord).ToList()
        );
    }
}
