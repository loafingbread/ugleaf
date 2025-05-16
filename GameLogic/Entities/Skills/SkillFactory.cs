using GameLogic.Entities.Skills;

public static class SkillFactory
{
    public static Skill CreateFromRecord(ISkillRecord record)
    {
        SkillConfig config = new(record);
        return new Skill(config);
    }
}