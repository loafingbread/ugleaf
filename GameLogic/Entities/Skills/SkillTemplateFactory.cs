namespace GameLogic.Entities.Skills;

using GameLogic.Registry;
using GameLogic.Usables;

public class SkillTemplateFactory : ITemplateFactory<SkillTemplateData, SkillTemplate>
{
    private static readonly UsableTemplateFactory _usableFactory = new();

    public SkillTemplate Create(ReferenceId id, SkillTemplateData data)
    {
        List<UsableTemplate> usables = new();
        foreach (UsableTemplateData usableData in data.Usables)
            usables.Add(_usableFactory.Create(ReferenceId.New(), usableData));
        return new SkillTemplate(id, data, usables);
    }
}
