namespace GameLogic.Entities.Skills;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public class Skill : IToData<SkillInstanceData>
{
    public SkillTemplate Template { get; }
    public int Level { get; private set; }
    public int CurrentCooldown { get; private set; }

    public string Id => Template.ToData().Id;
    public string Name => Template.Name;
    public TargeterConfig? Targeter => Template.Targeter;
    public List<UsableTemplate> Usables => Template.Usables;

    public Skill(SkillTemplate template, SkillInstanceData data)
    {
        Template = template;
        Level = data.Level;
        CurrentCooldown = data.CurrentCooldown;
    }

    public SkillInstanceData ToData() => new()
    {
        TemplateId = Template.ToData().Id,
        Level = Level,
        CurrentCooldown = CurrentCooldown,
    };
}
