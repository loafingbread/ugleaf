namespace GameLogic.Registry;

using GameLogic.Usables;

public class UsableReference : ReferenceBase<UsableTemplate, Usable>
{
    public UsableReference(
        EntityRegistry<UsableTemplate> entityRegistry,
        ReferenceSpec spec,
        UsableTemplate? value
    )
        : base(entityRegistry, spec, value) { }

    public override void ResolveDependencies(IRegistry registry)
    {
        this.Data.Resolve(this.Spec, this.GetUsableData(registry));
    }

    public override void Initialize()
    {
        this.Data.Initialize();
    }
}
