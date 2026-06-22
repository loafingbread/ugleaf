namespace GameLogic.Usables;

using GameLogic.Targeting;
using GameLogic.Usables.Effects;

public class UsableResult
{
    public UsableTemplate Usable { get; }
    public IUser User { get; }
    public ITargetable Target { get; }
    public List<IEffectResult> Results { get; } = new();

    public UsableResult(UsableTemplate usable, IUser user, ITargetable target)
    {
        Usable = usable;
        User = user;
        Target = target;
    }
}
