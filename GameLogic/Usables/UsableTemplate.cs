namespace GameLogic.Usables;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;

public class UsableTemplate : IToData<UsableTemplateData>
{
    private readonly UsableTemplateData _data;

    public ReferenceId Id { get; }
    public TargeterConfig Targeter { get; }
    public List<EffectTemplate> Effects { get; }

    public UsableTemplate(ReferenceId id, UsableTemplateData data, List<EffectTemplate> effects)
    {
        Id = id;
        _data = data;
        Targeter = new TargeterConfig(data.Targeter);
        Effects = effects;
    }

    public UsableResult Use(IUser user, ITargetable target)
    {
        UsableResult result = new(this, user, target);
        foreach (EffectTemplate effect in Effects)
            result.Results.Add(effect.EffectVariant.Compute(user, target));
        return result;
    }

    public UsableTemplateData ToData() => _data;
}
