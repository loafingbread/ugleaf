namespace GameLogic.Registry;

using GameLogic.Usables;
using GameLogic.Usables.Effects;

public class UsableReference : ReferenceBase<UsableTemplate, UsableData>
{
    public UsableReference(IRegistry registry, ReferenceSpec spec, UsableTemplate? value)
        : base(registry, spec, value) { }

    protected override UsableData InitData()
    {
        return new UsableData(this.Spec);
    }

    public override void ResolveDependencies(IRegistry registry)
    {
        this.Data.Resolve(this.Spec, this.GetUsableData(registry), this.GetEffectData(registry));
    }

    public override void Initialize()
    {
        if (this.Data is null)
        {
            throw new InvalidOperationException(
                "Usable data should be resolved before initializing the reference"
            );
        }

        if (this.Spec.Metadata.Kind == EReferenceKind.Instance)
        {
            Usable usable = new Usable(this.Data);
            this.Registry.TryAdd(
                (IReference<object, ReferenceSpec>)this,
                this.Spec.Metadata.ReferenceId
            );
            return;
        }

        UsableTemplate usableTemplate = new UsableTemplate(this.Data);
        this.Registry.TryAdd(
            (IReference<object, ReferenceSpec>)this,
            this.Spec.Metadata.ReferenceId
        );
    }

    private Func<ReferenceId?, UsableData> GetUsableData(IRegistry registry)
    {
        return (ReferenceId? referenceId) =>
        {
            var usableRef =
                registry.GetReference(referenceId) as IReference<UsableTemplate, UsableData>
                ?? throw new InvalidOperationException("Usable reference not found");

            usableRef.Resolve(registry);
            return usableRef.GetData();
        };
    }

    private Func<ReferenceId?, EffectData> GetEffectData(IRegistry registry)
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
