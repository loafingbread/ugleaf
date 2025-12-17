namespace GameLogic.Registry;

using GameLogic.Usables.Effects;

public class EffectReference : ReferenceBase<EffectTemplate, EffectData>
{
    public EffectReference(
        EntityRegistry<EffectTemplate> entityRegistry,
        ReferenceSpec spec,
        EffectTemplate? value
    )
        : base(entityRegistry, spec, value) { }

    protected override EffectData InitData()
    {
        return new EffectData(this.Spec);
    }

    public override void ResolveDependencies(IRegistry registry)
    {
        this.Data.Resolve(this.Spec, this.GetEffectData(registry));
    }

    public override void Initialize()
    {
        if (this.Data is null)
        {
            throw new InvalidOperationException(
                "Effect data should be resolved before initializing the reference"
            );
        }

        if (this.Spec.Metadata.Kind == EReferenceKind.Instance)
        {
            Effect effect = EffectFactory.CreateEffectFromData(this.Data);
            this.entityRegistry.TryAdd(effect, this.Spec.Metadata.ReferenceId);
            return;
        }

        EffectTemplate effectTemplate = new EffectTemplate(this.Data);
        this.entityRegistry.TryAdd(effectTemplate, this.Spec.Metadata.ReferenceId);
    }

    protected Func<ReferenceId?, EffectData> GetEffectData(IRegistry registry)
    {
        return (ReferenceId? referenceId) =>
        {
            var effectRef =
                registry.GetReference(referenceId) as IReference<EffectTemplate, EffectData>
                ?? throw new InvalidOperationException("Effect reference not found");

            effectRef.Resolve(registry);
            return effectRef.GetData();
        };
    }
}
