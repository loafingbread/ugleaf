namespace GameLogic.Usables.Usable;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;

public record UsableStateRef : InstanceRef<UsableIStateSpec> { }

public record UsableIStateSpec
{
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public List<string> Tags { get; private set; } = new();
    public TargeterData Targeter { get; private set; } = new();
    public List<EffectStateRef> Effects { get; private set; } = new();
}
