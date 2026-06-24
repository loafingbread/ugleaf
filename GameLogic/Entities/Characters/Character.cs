namespace GameLogic.Entities.Characters;

using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public class Character : IToInstanceData<CharacterInstanceData>, IUser, ITargetable
{
    public CharacterTemplate Template { get; }
    public StatBlock Stats { get; }
    public List<Skill> Skills { get; }
    public List<ActiveStatusEffect> ActiveStatusEffects { get; } = new();

    public string Id => Template.ToData().Id;
    public string Name => Template.Name;
    public EFaction Faction { get; set; } = EFaction.Neutral;

    public Character(CharacterTemplate template, CharacterInstanceData data)
    {
        Template = template;
        Stats = new StatBlock(template.ToData());
        Skills = template.Skills
            .Select(t => new Skill(t, new SkillInstanceData { TemplateId = t.ToData().Id }))
            .ToList();
    }

    public void AddStatusEffect(ActiveStatusEffect effect) =>
        ActiveStatusEffects.Add(effect);

    // IUser
    public bool CanUse(UsableTemplate usable) => true;

    // ITargetable
    public EFactionRelationship GetRelationTo(ITargeter targeter)
    {
        if (targeter is Character other)
        {
            if (other == this) return EFactionRelationship.Self;
            return other.Faction == Faction ? EFactionRelationship.Ally : EFactionRelationship.Enemy;
        }
        return EFactionRelationship.Enemy;
    }

    public CharacterInstanceData ToData() => new()
    {
        TemplateId = Template.ToData().Id,
        Skills = Skills.Select(s => s.ToData()).ToList(),
    };
}
