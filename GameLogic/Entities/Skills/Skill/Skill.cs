namespace GameLogic.Entities.Skills.Skill;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;
using GameLogic.Utils;

public class Skill : IDeepCopyable<Skill>
{
    public SkillTemplate Template { get; private set; }
    public SkillState State { get; private set; }

    public Skill(SkillTemplate template, SkillState state)
    {
        this.Template = template;
        this.State = state;
    }

    public Skill DeepCopy()
    {
        return new Skill(this.Template.DeepCopy(), this.State.DeepCopy());
    }

    public bool CanTarget() => this.State.Targeter is not NoTargeter;

    public bool CanUse() => this.State.Usables.Count > 0;

    public SkillTemplate GetTemplate() => this.Template;
}
