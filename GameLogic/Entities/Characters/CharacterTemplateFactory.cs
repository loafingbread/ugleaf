namespace GameLogic.Entities.Characters;

using GameLogic.Entities.Skills;
using GameLogic.Registry;

public class CharacterTemplateFactory : ITemplateFactory<CharacterTemplateData, CharacterTemplate>
{
    private static readonly SkillTemplateFactory _skillFactory = new();

    public CharacterTemplate Create(ReferenceId id, CharacterTemplateData data)
    {
        List<SkillTemplate> skills = new();
        foreach (SkillTemplateData skillData in data.Skills)
            skills.Add(_skillFactory.Create(ReferenceId.New(), skillData));
        return new CharacterTemplate(id, data, skills);
    }
}
