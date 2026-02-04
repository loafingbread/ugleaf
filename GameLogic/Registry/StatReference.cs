namespace GameLogic.Registry;

using GameLogic.Entities.Stats;

public class StatReference : ReferenceBase<Stat, StatData>
{
    public StatReference(EntityRegistry<Stat> entityRegistry, ReferenceSpec spec, Stat? value)
        : base(entityRegistry, spec, value) { }

    protected override StatData InitData()
    {
        return new StatData(this.Spec.Metadata.ReferenceId);
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

        StatModel statModel = StatFactory.CreateStatModelFromSpec(
            this.Data.Capabilities,
            new BuildContext(this.entityRegistry)
        );
        Stat stat = new Stat(this.Spec.Metadata.ReferenceId, new StatModel(this.Data));
        this.entityRegistry.TryAdd(stat, this.Spec.Metadata.ReferenceId);
    }
}
