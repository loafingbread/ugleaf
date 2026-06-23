namespace GameLogic.Entities.Characters;

using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;

public class Character : IToInstanceData<CharacterInstanceData>
{
    public CharacterTemplate Template { get; }
    public StatBlock Stats { get; }
    public List<Skill> Skills { get; }

    public string Id => Template.ToData().Id;
    public string Name => Template.Name;

    public Character(CharacterTemplate template, CharacterInstanceData data)
    {
        Template = template;
        Stats = new StatBlock(template.ToData());
        Skills = template.Skills
            .Select(t => new Skill(t, new SkillInstanceData { TemplateId = t.ToData().Id }))
            .ToList();
    }

    public CharacterInstanceData ToData() => new()
    {
        TemplateId = Template.ToData().Id,
        Skills = Skills.Select(s => s.ToData()).ToList(),
    };
}
