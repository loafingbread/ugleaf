namespace GameLogic.Events.Categories;

public abstract class SkillEvent : GameEvent<BuiltInEventCategory>
{
    // ✅ Overrides
    public string Name => $"{GetType().Name}: Skill[{Skill}], User[{Name}]";

    public string Skill { get; } = "";
    public string User { get; } = "";

    public SkillEvent(string skill, string user)
        : base(BuiltInEventCategory.Skill)
    {
        this.Skill = skill;
        this.User = user;
    }
}

public class SkillUseEvent : SkillEvent
{
    public SkillUseEvent(string skillName, string userName)
        : base(skillName, userName) { }
}
