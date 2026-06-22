namespace GameLogic.Usables;

using GameLogic.Registry;
using GameLogic.Usables.Effects;

public class UsableTemplateFactory : ITemplateFactory<UsableTemplateData, UsableTemplate>
{
    private static readonly EffectTemplateFactory _effectFactory = new();

    public UsableTemplate Create(ReferenceId id, UsableTemplateData data)
    {
        List<EffectTemplate> effects = new();
        foreach (EffectTemplateData effectData in data.Effects)
            effects.Add(_effectFactory.Create(ReferenceId.New(), effectData));
        return new UsableTemplate(id, data, effects);
    }
}
