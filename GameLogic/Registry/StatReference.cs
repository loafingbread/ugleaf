namespace GameLogic.Registry;

using GameLogic.Entities.Stats;

public class StatReference : ReferenceBase<Stat, StatData>
{
    public StatReference(IRegistry registry, ReferenceSpec spec, Stat? value)
        : base(registry, spec, value) { }

    protected override StatData InitData()
    {
        return new StatData(this.Spec.ReferenceMetadata.ReferenceId);
    }

    public override void ResolveDependencies(IRegistry registry)
    {
        this.Data.Resolve(this.Spec, this.GetStatData(registry));
    }

    private Func<ReferenceId?, StatData> GetStatData(IRegistry registry)
    {
        return (ReferenceId? referenceId) =>
        {
            var statRef =
                registry.GetReference(referenceId) as IReference<Stat, StatData>
                ?? throw new InvalidOperationException("Stat reference not found");

            statRef.Resolve(registry);
            return statRef.GetData();
        };
    }

    public override void Initialize()
    {
        if (this.Data is null)
        {
            throw new InvalidOperationException(
                "Stat data should be resolved before initializing the reference"
            );
        }

        StatModel statModel = StatFactory.CreateStatModelFromData(
            this.Data,
            new BuildContext(this.Registry)
        );
        this.Value = new Stat(this.Spec.ReferenceMetadata.ReferenceId, statModel);
        this.Registry.TryAdd(
            (IReference<object, ReferenceSpec>)this,
            this.Spec.ReferenceMetadata.ReferenceId
        );
    }
}
