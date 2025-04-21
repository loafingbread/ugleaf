namespace GameLogic.Events.SkillEvents
{
    public class SkillEvent : GameEvent<BuiltInEventCategory>
    {
        // ✅ Overrides
        public string Name => $"{GetType().Name}: {SkillName}";

        public string SkillName { get; } = "";
        public string TargetName { get; } = "";

        public SkillEvent(string skillName, string targetName)
            : base(BuiltInEventCategory.Skill)
        {
            this.SkillName = skillName;
            this.TargetName = targetName;
        }
    }
}
