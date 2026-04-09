namespace GameLogic.Actions.Effects;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Utils;

public class EffectState
{
    public ReferenceId ReferenceId { get; set; }
    public ReferenceId? DependencyId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public ITargeter Targeter { get; set; } = new NoTargeter(new Position(0, 0, 0));
    public List<IEffect> Effects { get; set; } = new();
}
