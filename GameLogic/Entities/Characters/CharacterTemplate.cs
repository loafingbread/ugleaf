namespace GameLogic.Entities.Characters;

using GameLogic.Entities.Skills;
using GameLogic.Registry;

public class CharacterTemplate : IToData<CharacterTemplateData>
{
    private readonly CharacterTemplateData _data;

    public ReferenceId Id { get; }
    public string Name => _data.Name;
    public List<SkillTemplate> Skills { get; }

    public CharacterTemplate(ReferenceId id, CharacterTemplateData data, List<SkillTemplate> skills)
    {
        Id = id;
        _data = data;
        Skills = skills;
    }

    public CharacterTemplateData ToData() => _data;
}
