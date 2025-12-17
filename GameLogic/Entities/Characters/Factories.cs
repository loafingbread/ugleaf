namespace GameLogic.Entities.Characters;

using System.Linq;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;
using GameLogic.Utils;

public static class CharacterFactory
{
    // public static Character CreateCharacterFromRecord(CharacterData record)
    // {
    //     return new Character(
    //         GameLogic.Registry.Ids.Instance(record.InstanceId),
    //         record.TemplateIdentifier,
    //         record.Name,
    //         record.Description,
    //         record.Tags,
    //         StatFactory.CreateStatBlockFromRecord(record),
    //         record
    //             .Skills.Select((SkillInstanceSpec skill) => SkillFactory.CreateSkillFromRecord(skill))
    //             .ToList()
    //     );
    // }

    public static Character CreateCharacterFromTemplate(CharacterTemplate template)
    {
        return template.Instantiate();
    }
}
