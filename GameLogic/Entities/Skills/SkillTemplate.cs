namespace GameLogic.Entities.Skills;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public class SkillTemplate : IToData<SkillTemplateData>
{
    private readonly SkillTemplateData _data;

    public ReferenceId Id { get; }
    public string Name => _data.Name;
    public TargeterConfig? Targeter { get; }
    public List<UsableTemplate> Usables { get; }

    public SkillTemplate(ReferenceId id, SkillTemplateData data, List<UsableTemplate> usables)
    {
        Id = id;
        _data = data;
        Targeter = data.Targeter != null ? new TargeterConfig(data.Targeter) : null;
        Usables = usables;
    }

    public SkillTemplateData ToData() => _data;
}
