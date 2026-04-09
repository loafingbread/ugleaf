namespace GameLogic.Registry;

using GameLogic.Actions.Effects;

public class EffectReference : ReferenceBase<Effect, EffectData>
{
    public EffectReference(IRegistry registry, ReferenceSpec spec, Effect? value)
        : base(registry, spec, value) { }

    protected override EffectData InitData() =>
        new()
        {
            Name = "",
            Description = "",
            Tags = new(),
            Type = "",
            Subtype = "",
            Config = new(),
        };

    public override void ResolveDependencies(IRegistry registry)
    {
        this.Data = EffectResolver.ToData(this.Spec, GetEffectData(registry));
    }

    public override void Initialize()
    {
        IEffectVariant variant = EffectVariantFactory.CreateVariant(this.Data);
        this.Value = new Effect(this.Spec.ReferenceMetadata.ReferenceId, variant);
        this.Registry.TryAdd(
            (IReference<object, ReferenceSpec>)this,
            this.Spec.ReferenceMetadata.ReferenceId
        );
    }

    private Func<ReferenceId?, EffectData> GetEffectData(IRegistry registry)
    {
        return (ReferenceId? referenceId) =>
        {
            var effectRef =
                registry.GetReference(referenceId) as IReference<Effect, EffectData>
                ?? throw new InvalidOperationException(
                    $"Effect reference not found for id: {referenceId}"
                );

            effectRef.Resolve(registry);
            return effectRef.GetData();
        };
    }
}
