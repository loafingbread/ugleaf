using GameLogic.Entities.Skills;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public static class SkillFactory
{
    public static Skill CreateSkillFromData(SkillData data)
    {
        return new Skill(data);
    }

    public static SkillTemplate CreateSkillTemplateFromData(SkillData data)
    {
        return new SkillTemplate(data);
    }
}
