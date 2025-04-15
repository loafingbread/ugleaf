namespace GameLogic.Events.SkillEvents
{
    public class SkillUsedEvent : GameEvent<EventCategory>
    {
        // ✅ Overrides
        public string Name => $"{GetType().Name}: {SkillName}";

        public string SkillName { get; } = "";
        public SkillUsedEvent(string skillName) : base(EventCategory.Skill)
        {
            SkillName = skillName;
        }
    }
}