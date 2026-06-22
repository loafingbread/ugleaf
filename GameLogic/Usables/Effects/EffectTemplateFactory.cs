namespace GameLogic.Usables.Effects;

using GameLogic.Registry;
using GameLogic.Usables.Effects.Variants;

public class EffectTemplateFactory : ITemplateFactory<EffectTemplateData, EffectTemplate>
{
    public EffectTemplate Create(ReferenceId id, EffectTemplateData data)
    {
        IEffectVariant variant = (data.Type, data.Subtype) switch
        {
            ("Attack", _) => new AttackEffectVariant(data),
            ("Heal", _) => new HealEffectVariant(data),
            ("Status", "Burn") => new BurnStatusVariant(data),
            ("Status", "Poison") => new PoisonStatusVariant(data),
            _ => throw new ArgumentException(
                $"Unknown effect type/subtype: {data.Type}/{data.Subtype}"
            ),
        };
        return new EffectTemplate(id, data, variant);
    }
}
