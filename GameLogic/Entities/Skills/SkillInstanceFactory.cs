namespace GameLogic.Entities.Skills;

using GameLogic.Registry;

public class SkillInstanceFactory : IInstanceFactory<SkillTemplate, SkillInstanceData, Skill>
{
    public Skill Create(SkillTemplate template, SkillInstanceData data) => new(template, data);
}
