namespace GameLogic.Registry;

using GameLogic.Usables.Effects;

public class EffectReference : ReferenceBase<EffectTemplate, IEffect>
{
    public EffectReference(ReferenceMetadata metadata, EffectTemplate? value)
        : base(metadata, value) { }
}