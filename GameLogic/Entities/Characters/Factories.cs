namespace GameLogic.Entities.Characters;

using System.Linq;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;

public static class CharacterFactory
{
    public static Character CreateCharacterFromRecord(CharacterRecord record)
    {
        return new Character(
            new GameLogic.Registry.InstanceId(record.InstanceId),
            new GameLogic.Registry.TemplateId(record.TemplateId),
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
            template.Id,
            template.Metadata.Name,
            template.Metadata.Description,
            template.Metadata.Tags,
            StatFactory.CreateStatBlockFromRecord(template.Config),
            template
                .Config.Skills.Select(
                    (SkillRecord skill) => SkillFactory.CreateSkillFromRecord(skill)
                )
                .ToList()
        );
    }

    public static CharacterTemplate CreateCharacterTemplateFromRecord(
        CharacterTemplateRecord record
    )
    {
        return new CharacterTemplate(record);
    }
}
